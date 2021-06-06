using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;
using LLGenerator.Types;

namespace LLGenerator.TableGenerator
{
    public static class TableBuilder
    {
        public static ImmutableList<TableRule> Build(ImmutableList<DirRule> dirRules)
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
                    switch (item.Type)
                    {
                        case ElementType.Terminal:
                        case ElementType.End:
                            dirSet.Add(item.Value);
                            break;
                        case ElementType.Empty:
                            dirSet = dRule.Dirs;
                            break;
                        case ElementType.NonTerminal:
                            dirSet = dirRules.Where(x => x.NonTerminal == item.Value)
                                .SelectMany(x => x.Dirs).ToHashSet();
                            break;
                    }

                    var isLast = index + 1 == dRule.Items.Count;
                    int? ptr = null;
                    if (item.Type is ElementType.NonTerminal)
                    {
                        ptr = table.First(x => x.NonTerminal == item.Value).Id;
                    }
                    else if (!isLast)
                    {
                        ptr = globalId + 1;
                    }

                    addTable.Add(new TableRule
                    {
                        Id = newRuleId,
                        NonTerminal = item.Value,
                        DirSet = dirSet,
                        GoTo = ptr,
                        IsError = true,
                        IsShift = item.Type is ElementType.Terminal or ElementType.End,
                        MoveToStack = item.Type is ElementType.NonTerminal && !isLast,
                        IsEnd = item.Type is ElementType.End
                    });
                    if (index == 0) table[i].GoTo = newRuleId;
                }
            }

            table.AddRange(addTable);
            return table.ToImmutableList();
        }
    }
}