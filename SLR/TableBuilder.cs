using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public static class TableBuilder
    {

        
        public static void CreateTable(ImmutableList<Rule> rules) 
        {
            
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
            
            var tableRules = new List<TableRule>();
            var keyQueue = new Queue<RuleItems>();
            var queueBlackList = new HashSet<RuleItems>();
            {
                var itemId = new RuleItemId(0, -1);
                var tableRule = new TableRule(rules[itemId.RuleIndex].NonTerminal, valueKeys);

                AddNext(tableRule, itemId);
                AddToQueue(tableRule);
            }

            while (keyQueue.Count > 0)
            {
                var items = keyQueue.Dequeue();
                queueBlackList.Add(items);
                var key = string.Join("", items.Select(x => x.ToString()));
                var tableRule = new TableRule(key, valueKeys);
                foreach (var item in items)
                {
                    // Last item
                    if (rules[item.Id.RuleIndex].Items.Count <= item.Id.ItemIndex + 1)
                    {
                        
                    }
                    else // Not last item
                    {
                        AddNext(tableRule, item.Id);
                    }
                }
                AddToQueue(tableRule);
            }
            
            Console.WriteLine();
            
            void AddNext(TableRule tableRule, RuleItemId itemId)
            {
                var next = rules[itemId.RuleIndex].Items[itemId.ItemIndex + 1];
                tableRule.QuickAdd(next);
                foreach (var rule in rules.Where(x => x.NonTerminal == next.Value))
                {
                    tableRule.QuickAdd(rule.Items[0]);
                }
                tableRules.Add(tableRule);
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
    }
}