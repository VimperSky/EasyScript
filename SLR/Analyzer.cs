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
        public static void Analyze(IEnumerable<string> input, List<TableRule> table, List<Rule> rules)
        {
            var left = new Stack<string>();
            var right = new Stack<string>();
            var inputStack = new Stack<string>();
            foreach (var inp in input.Reverse())
                inputStack.Push(inp);

            right.Push(table.First().Key);
            while (true)
                try
                {
                    var ch = inputStack.Count > 0 ?  inputStack.Pop() : "";
                    var values = table.First(x => x.Key == right.Peek()).Values;
                    var items = ch == ""
                        ? values.Where(x => x.Key == Constants.EndSymbol).ToList()
                        : values.Where(x => x.Key == ch).ToList();

                    if (items.Count == 0)
                        throw new Exception("Items are empty");

                    var elements = items.First().Value;
                    
                    switch (elements.Count)
                    {
                        case > 0 when elements.First().Value == "OK":
                            Console.WriteLine("Analyzer correct!");
                            return;
                        // Иначе
                        case > 0 when elements.First().Type is ElementType.Collapse:
                        {
                            if (ch != "") inputStack.Push(ch);

                            // номер свертки
                            var ruleNumber =
                                int.Parse(elements.First().Value.Substring(1, elements.First().Value.Length - 1)) - 1;
                            var rule = rules[ruleNumber];

                            if (rule.Items[0].Type is not ElementType.Empty)
                                for (var i = 0; i < rule.Items.Count && rule.Items[i].Type is not ElementType.End; i++)
                                {
                                    left.Pop();
                                    right.Pop();
                                }

                            if (right.Count == 1 && left.Count == 0 && inputStack.Count == 0)
                            {
                                Console.WriteLine("Analyzer correct!");
                                return;
                            }
                            inputStack.Push(rule.NonTerminal);
                            break;
                        }
                        default:
                            right.Push(elements.ToString());
                            left.Push(ch);
                            break;
                    }

                    Console.WriteLine($"Left [{string.Join(", ", left.ToArray())}]" +
                                      $" Input [{string.Join(" ", inputStack.ToArray())}]" +
                                      $" Right [{string.Join(", ", right.ToArray())}]");
                }
                catch (Exception e)
                {
                    throw new ArgumentException("[Syntax Analyzer Error] " + e + "\r\n*** Analyzer State ***" +
                                                $"\r\nLeft [{string.Join(", ", left.ToArray())}]" +
                                                $"\r\nInput [{string.Join(" ", inputStack.ToArray())}]" +
                                                $"\r\nRight [{string.Join(", ", right.ToArray())}]");
                }
        }
    }
}