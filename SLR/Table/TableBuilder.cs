using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SLR.Types;

namespace SLR.Table
{
    public class TableBuilder
    {
        private readonly ImmutableList<Rule> _rules;
        private readonly ImmutableList<string> _valueKeys;

        public TableBuilder(ImmutableList<Rule> rules)
        {
            _rules = rules;
            var valueKeys = new HashSet<string>();
            foreach (var rule in rules)
            {
                valueKeys.Add(rule.NonTerminal);
                foreach (var ruleItem in rule.Items.Where(ruleItem =>
                    ruleItem.Type != ElementType.Empty && ruleItem.Type != ElementType.End))
                    valueKeys.Add(ruleItem.Value);
            }

            valueKeys.Add(Constants.EndSymbol);
            _valueKeys = valueKeys.ToImmutableList();
        }

        private TableRule CreateTableRule(string key) => new(key, _valueKeys);
        private RuleItem GetItem(int ruleIndex, int itemIndex) => _rules[ruleIndex].Items[itemIndex];

        public ImmutableList<TableRule> CreateTable()
        {
            // Таблица и список правил на разбор и добавление в таблицу
            var tableRules = new List<TableRule>();
            var pendingItems = new Queue<RuleItems>();
            
            { // Начальный проход с первого нетерминала
                var tableRule = CreateTableRule(_rules[0].NonTerminal);
                ProcessFirst(tableRule, _rules[0].NonTerminal);
                UpdatePendingItems(tableRule);
                tableRules.Add(tableRule);
            }
            
            // Проход по остальным правилам, берем последовательно с очереди
            while (pendingItems.Count > 0)
            {
                var items = pendingItems.Dequeue();
                var key = string.Join("", items.Select(x => x.ToString()));
                if (tableRules.Any(x => x.Key == key))
                    continue;
            
                var tableRule = CreateTableRule(key);
                foreach (var item in items)
                {
                    ProcessFollow(tableRule, item);
                }
            
                tableRules.Add(tableRule);
                UpdatePendingItems(tableRule);
            }

            return tableRules.ToImmutableList();
            
            void UpdatePendingItems(TableRule tableRule)
            {
                foreach (var item in tableRule.Values
                    .Where(x => x.Value.Count > 0))
                {
                    var value = item.Value;
                    if (tableRules.Any(x => x.Key == value.ToString()))
                        continue;
                    
                    if (value.Any(x => x.Value.StartsWith("R")))
                        continue;
                    
                    pendingItems.Enqueue(value);
                }
            }
        }
        
        private void ProcessFirst(TableRule tableRule, string nonTerm)
        {
            foreach (var rules in _rules.Where(x => x.NonTerminal == nonTerm))
            {
                var first = rules.Items[0];
                if (first.Type is ElementType.Terminal or ElementType.NonTerminal)
                {
                    tableRule.QuickAdd(first);
                    if (first.Type is ElementType.NonTerminal && nonTerm != first.Value)
                    {
                        ProcessFirst(tableRule, first.Value);
                    }
                    continue;
                }
                
                if (first.Type == ElementType.Empty)
                {
                    var next = FindNextRecursive(nonTerm);
                    foreach (var nextItem in next)
                    {
                        ProcessFollow(tableRule, nextItem);
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает все элементы, идущие после нетерминала из которого выводится e
        /// </summary>
        /// <param name="tableRule"></param>
        /// <param name="item"></param>
        private void ProcessFollow(TableRule tableRule, RuleItem item)
        {
            // Это последний элемент в правиле
            if (item.ItemIndex >= _rules[item.RuleIndex].Items.Count - 1)
            {
                var nextItems = FindNextRecursive(_rules[item.RuleIndex].NonTerminal);
                foreach (var nextItem in nextItems)
                    // Добавление элементов, чтобы не повторялись в одной ячейке
                    tableRule.QuickFold(nextItem.Value, item.RuleIndex + 1);
                return;
            }
            
            var next = GetItem(item.RuleIndex, item.ItemIndex + 1);
            if (next.Value == Constants.EndSymbol)
            {
                tableRule.QuickFold(Constants.EndSymbol, item.RuleIndex + 1);
            }
            else
            {
                ProcessFirst(tableRule, item.Value);
            }
        }

        
        
        private IEnumerable<RuleItem> FindNextRecursive(string nonTerm)
        {
            return FindUp(nonTerm, new HashSet<int>());
        }

        private IEnumerable<RuleItem> FindUp(string nonTerm, ISet<int> history)
        {
            var returns = new HashSet<RuleItem>();
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                for (var j = 0; j < rule.Items.Count; j++)
                    if (rule.Items[j].Value == nonTerm)
                    {
                        if (++j < rule.Items.Count)
                        {
                            returns.Add(rule.Items[j]);
                        }
                        else
                        {
                            if (history.Contains(i)) return returns;
                            history.Add(i);
                            var nextReturns = FindUp(rule.NonTerminal, history);
                            foreach (var item in nextReturns)
                                returns.Add(item);
                        }
                    }
            }

            return returns;
        }
    }
}