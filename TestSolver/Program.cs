using System;
using System.Collections.Immutable;
using System.IO;
using LLGenerator.SetsParser;
using LLGenerator.SyntaxAnalyzer;
using LLGenerator.TableGenerator;

namespace TestSolver
{
    internal static class Program
    {
        private static void Main()
        {
            var rulesStream = File.OpenRead("rules.txt");
            var dirRules = SetsParser.DoParse(rulesStream);
            Console.WriteLine("Rules:");
            foreach (var rule in dirRules) Console.WriteLine(rule);
            if (!SetsParser.IsLLFirst(dirRules))
            {
                Console.WriteLine("Not LL1 grammar");
                return;
            }

            var tableRules = TableGenerator.Parse(dirRules);
            CsvExport.SaveToCsv(tableRules);
            var input = File.ReadAllText("input.txt").Split(" ", StringSplitOptions.TrimEntries);
            Console.WriteLine($"Input: {string.Join(" ", input)}");
            ImmutableList<int> history;
            try
            {
                history = SyntaxAnalyzer.Analyze(input, tableRules);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Correct! History: [{string.Join(", ", history)}]");
        }
    }
}