﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public class Analyzer
    {
        private readonly string[] _input;
        private readonly ImmutableList<Rule> _rules;
        private readonly ImmutableList<TableRule> _tableRules;

        public Analyzer(Stream stream, ImmutableList<TableRule> table, ImmutableList<Rule> rules)
        {
            _input = InputParser(stream);
            _tableRules = table;
            _rules = rules;
        }

        private static string[] InputParser(Stream stream)
        {
            using var sr = new StreamReader(stream);
            string line;
            string[] split = { };
            while ((line = sr.ReadLine()) != null)
            {
                split = line.Split();
                break;
            }

            return split;
        }


        public void Analyze()
        {
            var left = new Stack<string>();
            var right = new Stack<string>();
            var inputStack = new Stack<string>();
            foreach (var input in _input.Reverse())
                inputStack.Push(input);

            right.Push(_tableRules.First().Key);
            while (true)
                try
                {
                    var character = "";
                    if (inputStack.Count > 0) character = inputStack.Pop();
                    var values = _tableRules.First(x => x.Key == right.Peek()).Values;
                    var items = character == ""
                        ? values.Where(x => x.Key == Constants.EndSymbol).ToList()
                        : values.Where(x => x.Key == character).ToList();

                    if (items.Count == 0)
                        throw new Exception("Items are empty");

                    var elements = items.First().Value;
                    // Если свертка
                    if (elements.First().Value.StartsWith("R"))
                    {
                        if (character != "")
                            inputStack.Push(character);

                        // номер свертки
                        var ruleNumber =
                            int.Parse(elements.First().Value.Substring(1, elements.First().Value.Length - 1)) - 1;
                        var rule = _rules[ruleNumber];

                        if (rule.Items[0].Value != Constants.EmptySymbol)
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