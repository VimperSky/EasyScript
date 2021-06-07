using System;
using System.Collections.Generic;
using System.Linq;
using LL.Types;

namespace LL.SyntaxAnalyzer
{
    public static class SyntaxAnalyzer
    {
        public static List<int> Analyze(string[] input, List<TableRule> table)
        {
            var stack = new Stack<int>();
            var inputQ = new Queue<string>(input);
            var index = 1;
            var history = new List<int>();
            var inItem = inputQ.Peek();
            while (true)
            {
                history.Add(index);
                var tableItem = table[index - 1];
                if (!tableItem.DirSet.Contains(inItem))
                {
                    if (tableItem.IsError)
                        GenerateException($"DirSet doesn't contain token: {inItem ?? "NULL"}. " +
                                          $"TableItem is: {tableItem}.");
                    index++;
                    continue;
                }

                if (tableItem.IsShift)
                    if (inputQ.Count == 0)
                    {
                        GenerateException("Input is empty but we need to shift.");
                    }
                    else
                    {
                        inputQ.Dequeue();
                        inItem = inputQ.Count > 0 ? inputQ.Peek() : null;
                    }

                if (tableItem.MoveToStack)
                    stack.Push(index + 1);
                if (tableItem.GoTo != null)
                {
                    index = tableItem.GoTo.Value;
                }
                else
                {
                    if (stack.Count > 0)
                        index = stack.Pop();
                    else if (inItem == null && tableItem.IsEnd)
                        break;
                    else
                        GenerateException("GoTo is null, stack is empty but other conditions are not met.");
                }
            }

            void GenerateException(string err)
            {
                throw new ArgumentException("[Syntax Analyzer Error] " + err +
                                            $"\nToken Number: {input.Length - inputQ.Count}, " +
                                            $"Stack: [{string.Join(", ", stack)}], InputQ: [{string.Join(", ", inputQ)}], \nHistory: [{string.Join(", ", history)}]");
            }

            return history.ToList();
        }
    }
}