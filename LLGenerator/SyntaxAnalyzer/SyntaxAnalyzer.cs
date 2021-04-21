using System;
using System.Collections.Generic;
using LLGenerator.Entities;

namespace LLGenerator.SyntaxAnalyzer
{
    public static class SyntaxAnalyzer
    {
        public static void Analyze(IEnumerable<string> input, List<TableRule> table)
        {
            var stack = new Stack<int>();
            var inputQ = new Queue<string>(input);
            var index = 1;

            var inItem = inputQ.Peek();
            while (true)
            {
                var tableItem = table[index - 1];
                if (!tableItem.DirSet.Contains(inItem))
                {
                    if (tableItem.IsError)
                        throw new Exception("SyntaxAnalyzer error: dirset doesn't contain this char.\n" +
                                            $"STATE: Token: [{inItem}], tableItem: [{tableItem}]," +
                                            $"stack: {string.Join(", ", stack)}, " +
                                            $"input: {string.Join("", inputQ)}\n");

                    index++;
                    continue;
                }


                if (tableItem.IsShift)
                    if (inputQ.Count == 0)
                        throw new Exception("SyntaxAnalyzer error: we need next token but input is empty\n" +
                                            $"STATE: Token: [{inItem}], tableItem: [{tableItem}], " +
                                            $"stack: {string.Join(", ", stack)}, " +
                                            $"input: {string.Join("", inputQ)}\n");
                    else
                    {
                        inputQ.Dequeue();
                        if (inputQ.Count == 0)
                        {
                            
                        }
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
                        throw new Exception(
                            "SyntaxAnalyzer error: stack is empty but finish conditions are not met.\n" +
                            $"STATE: Token: [{inItem}], tableItem: [{tableItem}], " +
                            $"stack: {string.Join(", ", stack)}, " +
                            $"input: {string.Join("", inputQ)}\n");
                }
            }
        }
    }
}