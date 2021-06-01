﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public class TableBuilder
    {
        private readonly ImmutableList<Rule> _rules;
        private readonly ImmutableHashSet<string> _valueKeys;
        public TableBuilder(ImmutableList<Rule> rules)
        {
            _rules = rules;
            var valueKeys = new HashSet<string>();
            foreach (var rule in rules)
            {
                valueKeys.Add(rule.NonTerminal);
                foreach (var ruleItem in rule.Items)
                {
                    if (ruleItem != Constants.EmptySymbol && ruleItem != Constants.EndSymbol)
                    {
                        valueKeys.Add(ruleItem.Value);
                    }
                }
            }
            valueKeys.Add(Constants.EndSymbol);
            _valueKeys = valueKeys.ToImmutableHashSet();
        }

        public ImmutableHashSet<TableRule> CreateTable() 
        {
            var tableRules = new HashSet<TableRule>();
            var keyQueue = new Queue<RuleItems>();
            var queueBlackList = new HashSet<RuleItems>();
            {
                var itemId = new RuleItemId(0, -1);
                var tableRule = new TableRule(_rules[itemId.RuleIndex].NonTerminal, _valueKeys);

                AddNext(tableRule, itemId);
                AddToQueue(tableRule);
                tableRules.Add(tableRule);
            }

            while (keyQueue.Count > 0)
            {
                var items = keyQueue.Dequeue();
                queueBlackList.Add(items);
                var key = string.Join("", items.Select(x => x.ToString()));
                if (tableRules.Any(x => x.Key == key))
                {
                    continue;
                }
                var tableRule = new TableRule(key, _valueKeys);
                foreach (var item in items)
                {
                    // Последний элемент, делаем свертку
                    if (_rules[item.Id.RuleIndex].Items.Count <= item.Id.ItemIndex + 1)
                    {
                        // Ищем всё, где встречается данный элемент
                        var nextItems = FindNextRecursive(item.Value);
                        foreach (var nextItem in nextItems)
                        {
                            tableRule.Values[nextItem.Value].Add(new RuleItem("R" + item.Id.RuleIndex) {Id = nextItem.Id});
                        }
                    }
                    else // элемент не последний, добавляем следующий в строку
                    {
                        AddNext(tableRule, item.Id);
                        tableRules.Add(tableRule);
                    }
                }
                AddToQueue(tableRule);
            }

            
            Console.WriteLine($"   | {string.Join("   ", _valueKeys)}");
            Console.WriteLine(string.Join("\r\n", tableRules));
            return tableRules.ToImmutableHashSet();
            
            void AddNext(TableRule tableRule, RuleItemId itemId)
            {
                var next = _rules[itemId.RuleIndex].Items[itemId.ItemIndex + 1];
                tableRule.QuickAdd(next);
                foreach (var rule in _rules.Where(x => x.NonTerminal == next.Value))
                {
                    tableRule.QuickAdd(rule.Items[0]);
                }
            }

            void AddToQueue(TableRule tableRule)
            {
                foreach (var item in tableRule.Values
                    .Where(x => x.Value.Count > 0))
                {
                    var value = item.Value;
                    if (!queueBlackList.Contains(value))
                        keyQueue.Enqueue(value);
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
                            returns.Add((rule.Items[j]));
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