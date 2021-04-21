using System;
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
            foreach (var rule in dirRules)
                Console.WriteLine(rule);

            var tableRules = TableGenerator.Parse(dirRules);
            CsvExport.SaveToCsv(tableRules);
            var input = File.ReadAllText("input.txt").Split(" ", StringSplitOptions.TrimEntries);
            Console.WriteLine($"Input: {string.Join(" ", input)}");
            try
            {
                SyntaxAnalyzer.Analyze(input, tableRules);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Input file is not correct: {ex}");
                return;
            }
            
            Console.WriteLine("Input is correct!");

        }
    }
}