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
                    ruleItem != Constants.EmptySymbol && ruleItem != Constants.EndSymbol))
                    valueKeys.Add(ruleItem.Value);
            }

            valueKeys.Add(Constants.EndSymbol);
            _valueKeys = valueKeys.ToImmutableList();
        }

        public ImmutableList<TableRule> CreateTable()
        {
            var tableRules = new List<TableRule>();
            var pendingItems = new Queue<RuleItems>();
            {
                var itemId = new RuleItemId(0, -1);
                var tableRule = new TableRule(_rules[itemId.RuleIndex].NonTerminal, _valueKeys);

                AddNext(tableRule, itemId);
                UpdatePendingItems(tableRule);
                tableRules.Add(tableRule);
            }

            while (pendingItems.Count > 0)
            {
                var items = pendingItems.Dequeue();
                var key = string.Join("", items.Select(x => x.ToString()));
                if (tableRules.Any(x => x.Key == key))
                    continue;

                var tableRule = new TableRule(key, _valueKeys);
                foreach (var item in items)
                    // Последний элемент
                    if (_rules[item.Id.RuleIndex].Items.Count <= item.Id.ItemIndex + 1)
                    {
                        var nextItems = FindNextRecursive(_rules[item.Id.RuleIndex].NonTerminal);
                        foreach (var nextItem in nextItems)
                            // Добавление элементов, чтобы не повторялись в одной ячейке
                            tableRule.QuickFold(nextItem, new RuleItem("R" + (item.Id.RuleIndex + 1)));
                    }
                    // Конец цепочки
                    else if (_rules[item.Id.RuleIndex].Items[item.Id.ItemIndex + 1].Value == Constants.EndSymbol)
                    {
                        tableRule.Values[Constants.EndSymbol].Add(new RuleItem("R" + (item.Id.RuleIndex + 1)));
                    }
                    // Не последний элемент
                    else
                    {
                        AddNext(tableRule, item.Id);
                    }

                tableRules.Add(tableRule);
                UpdatePendingItems(tableRule);
            }

            return tableRules.ToImmutableList();

            void AddNext(TableRule tableRule, RuleItemId itemId)
            {
                var next = _rules[itemId.RuleIndex].Items[itemId.ItemIndex + 1];
                tableRule.QuickAdd(next);
                foreach (var rule in _rules.Where(x => x.NonTerminal == next.Value))
                    if (next.Value != rule.Items[0].Value && !rule.Items[0].IsTerminal)
                    {
                        AddNext(tableRule, new RuleItemId(rule.Items[0].Id.RuleIndex, rule.Items[0].Id.ItemIndex - 1));
                    }
                    else if (rule.Items[0].Value == Constants.EmptySymbol)
                    {
                        Console.WriteLine(rule.Items[0].ToString());
                        var nextItems = FindNextRecursive(rule.NonTerminal);
                        foreach (var nItem in nextItems)
                        {
                            if (nItem.IsTerminal)
                            {
                                tableRule.Values[nItem.Value].Add(new RuleItem("R" + (rule.Items[0].Id.RuleIndex + 1)));
                            }
                            else
                            {
                                // TODO: Добавить новую функцию
                            }
                        }
                    }
                    else
                    {
                        tableRule.QuickAdd(rule.Items[0]);
                    }
            }

            void UpdatePendingItems(TableRule tableRule)
            {
                foreach (var item in tableRule.Values
                    .Where(x => x.Value.Count > 0))
                {
                    var value = item.Value;
                    if (tableRules.All(x => x.Key != value.ToString()) && !value[0].Value.Contains("R"))
                        pendingItems.Enqueue(value);
                }
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