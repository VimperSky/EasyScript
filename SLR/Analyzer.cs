using System;
using System.Collections;
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
        private readonly ImmutableHashSet<TableRule> _tableRules;

        public Analyzer(Stream stream, ImmutableHashSet<TableRule> table, ImmutableList<Rule> rules)
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
            foreach (var input in _input.Reverse()) inputStack.Push(input);
            right.Push(_tableRules.First().Key);
            while (true)
            {
                var character = "";
                if (inputStack.Count > 0) character = inputStack.Pop();
                var row = _tableRules.Where(x => x.Key == right.Peek());
                var rows = row.First().Values;
                var cell = character == ""
                    ? rows.Where(x => x.Key == Constants.EndSymbol).ToList()
                    : rows.Where(x => x.Key == character).ToList();

                if (cell.Count == 0)
                {
                    Console.WriteLine("\nError");
                    return;
                }

                var element = cell.First().Value;
                if (element.First().Value.Contains("R"))
                {
                    if (character != "") inputStack.Push(character);

                    var number = int.Parse(element.First().Value.Substring(1, element.First().Value.Length - 1)) - 1;
                    var rule = _rules[number];
                    if (rule.Items[0].Value != Constants.EmptySymbol)
                        for (var i = 0; i < rule.Items.Count; i++)
                        {
                            left.Pop();
                            right.Pop();
                        }

                    if (right.Count == 1 && left.Count == 0 && inputStack.Count == 0)
                    {
                        Console.WriteLine("Correct");
                        return;
                    }

                    inputStack.Push(rule.NonTerminal);
                }
                else
                {
                    right.Push(string.Join("", element));
                    left.Push(character);
                }

                Console.WriteLine(
                    $"Left [{string.Join(", ", left.ToArray())}] Input [{string.Join(" ", inputStack.ToArray())}] Right [{string.Join(", ", right.ToArray())}]");
            }
        }
    }
}