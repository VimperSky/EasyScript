using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator;
using Generator.Types;

namespace SLR
{
    public class Analyzer
    {
        public static void Analyze(string[] input, ImmutableList<TableRule> table, ImmutableList<Rule> rules)
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
                    var character = "";
                    if (inputStack.Count > 0) character = inputStack.Pop();
                    var values = table.First(x => x.Key == right.Peek()).Values;
                    var items = character == ""
                        ? values.Where(x => x.Key == Constants.EndSymbol).ToList()
                        : values.Where(x => x.Key == character).ToList();

                    if (items.Count == 0)
                        throw new Exception("Items are empty");

                    var elements = items.First().Value;
                    // Если свертка
                    if (elements.First().Type is ElementType.Collapse)
                    {
                        if (character != "")
                            inputStack.Push(character);

                        // номер свертки
                        var ruleNumber =
                            int.Parse(elements.First().Value.Substring(1, elements.First().Value.Length - 1)) - 1;
                        var rule = rules[ruleNumber];

                        if (rule.Items[0].Type is not ElementType.Empty)
                            for (var i = 0; i < rule.Items.Count; i++)
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
                    }
                    // Иначе
                    else
                    {
                        right.Push(elements.ToString());
                        left.Push(character);
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