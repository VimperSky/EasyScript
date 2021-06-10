using System;
using System.Collections.Generic;
using System.Linq;
using Generator;
using Generator.Types;
using SLR.Types;

namespace SLR
{
    public static class Analyzer
    {
        public static void Analyze(IEnumerable<string> input, List<TableRule> table)
        {
            var dataStack = new Stack<string>();
            var inputStack = new Stack<string>();
            foreach (var inp in input.Reverse())
                inputStack.Push(inp);

            dataStack.Push(table.First().Key);
            while (true)
                try
                {
                    var firstData = dataStack.Pop();
                    var tableLine = table.First(x => x.Key == firstData);
                    var tableItem = tableLine.Values
                        .First(x => x.Key == inputStack.Peek());

                    if (tableItem.Value.Count == 0)
                    {
                        var value = tableItem.Value[0];
                        if (value.Value == "OK")
                        { 
                            Console.WriteLine("Analyzer correct!");
                            return;
                        }
                        
                        if (value.Type is ElementType.Collapse)
                        {
                            var elements = value.Value.Split("-");
                            inputStack.Push(elements[0]); // Collapse NonTerminal
                            for (var i = 0; i < int.Parse(elements[1]); i++) // Collapse item count
                            {
                                dataStack.Pop();
                            }
                        }
                    }
                    else
                    {
                        inputStack.Pop();
                        dataStack.Push(tableItem.Value.ToString());
                    }
                   
                    // var ch = inputStack.Count > 0 ? inputStack.Pop() : "";
                    //
                    // var tableValuesWithKey = table.First(x => x.Key == right.Peek()).Values;
                    // var tableItems = ch == ""
                    //     ? tableValuesWithKey.Where(x => x.Key == Constants.EndSymbol).ToList()
                    //     : tableValuesWithKey.Where(x => x.Key == ch).ToList();
                    //
                    // if (tableItems.Count == 0)
                    //     throw new Exception("Items are empty");
                    //
                    // var elements = tableItems.First().Value;
                    //
                    // if (elements.Count == 0)
                    //     continue;
                    //
                    // var firstElement = elements.First();
                    // if (firstElement.Value == "OK")
                    // {
                    //     Console.WriteLine("Analyzer correct!");
                    //     return;
                    // }
                    //
                    // if (firstElement.Type is ElementType.Collapse)
                    // {
                    //     if (ch != "") inputStack.Push(ch);
                    //
                    //     // номер свертки
                    //     var ruleNumber =
                    //         int.Parse(elements.First().Value.Substring(1, elements.First().Value.Length - 1)) - 1;
                    //     var rule = rules[ruleNumber];
                    //
                    //     if (rule.Items[0].Type is not ElementType.Empty)
                    //         for (var i = 0; i < rule.Items.Count && rule.Items[i].Type is not ElementType.End; i++)
                    //         {
                    //             dataStack.Pop();
                    //             right.Pop();
                    //         }
                    //
                    //     if (right.Count == 1 && dataStack.Count == 0 && inputStack.Count == 0)
                    //     {
                    //         Console.WriteLine("Analyzer correct!");
                    //         return;
                    //     }
                    //
                    //     inputStack.Push(rule.NonTerminal);
                    //     continue;
                    // }
                    
                    Console.WriteLine($"Data: [{string.Join(", ", dataStack.ToArray())}]" +
                                      $"\r\nInput: [{string.Join(" ", inputStack.ToArray())}]");
                }
                catch (Exception e)
                {
                    throw new ArgumentException("[Syntax Analyzer Error] " + e + "\r\n*** Analyzer State ***" +
                                                $"\r\nData: [{string.Join(", ", dataStack.ToArray())}]" +
                                                $"\r\nInput: [{string.Join(" ", inputStack.ToArray())}]");
                }
        }
    }
}