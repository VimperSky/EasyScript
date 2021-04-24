using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.TableGenerator
{
    public static class TableGenerator
    {
        public static ImmutableList<TableRule> Parse(ImmutableList<DirRule> dirRules)
        {
            var table = new List<TableRule>();
            var globalId = 0;
            for (; globalId < dirRules.Count; globalId++)
            {
                var dRule = dirRules[globalId];

                table.Add(new TableRule
                {
                    Id = globalId + 1, NonTerminal = dRule.NonTerminal, DirSet = dRule.Dirs,
                    IsError = globalId + 1 >= dirRules.Count || dirRules[globalId + 1].NonTerminal != dRule.NonTerminal
                });
            }

            var addTable = new List<TableRule>();
            for (var i = 0; i < dirRules.Count; i++)
            {
                var dRule = dirRules[i];
                for (var index = 0; index < dRule.Items.Count; index++)
                {
                    var newRuleId = ++globalId;
                    var item = dRule.Items[index];
                    var dirSet = new HashSet<string>();
                    if (item.IsTerminal)
                    {
                        if (item.Value == Constants.EmptySymbol)
                            dirSet = dRule.Dirs;
                        else
                            dirSet.Add(item.Value);
                    }
                    else
                    {
                        dirSet = dirRules.Where(x => x.NonTerminal == item.Value).SelectMany(x => x.Dirs).ToHashSet();
                    }

                    var isLast = index + 1 == dRule.Items.Count;
                    int? ptr = null;
                    if (item.IsTerminal)
                    {
                        if (!isLast)
                            ptr = globalId + 1;
                    }
                    else
                    {
                        ptr = table.First(x => x.NonTerminal == item.Value).Id;
                    }

                    addTable.Add(new TableRule
                    {
                        Id = newRuleId,
                        NonTerminal = item.Value,
                        DirSet = dirSet,
                        GoTo = ptr,
                        IsError = true,
                        IsShift = item.IsTerminal && item.Value != Constants.EmptySymbol,
                        MoveToStack = !item.IsTerminal && !isLast,
                        IsEnd = item.Value == Constants.NewLineSymbol
                    });

                    if (index == 0)
                        table[i].GoTo = newRuleId;
                }
            }

            table.AddRange(addTable);
            return table.ToImmutableList();
        }
    }
}