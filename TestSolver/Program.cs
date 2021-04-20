using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using LLGenerator.Entities;
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
            foreach (var rule in dirRules)
                Console.WriteLine(rule);

            var tableRules = TableGenerator.Parse(dirRules);

            try
            {
                var input = File.ReadAllText("input.txt").Split(" ", StringSplitOptions.TrimEntries);
                SyntaxAnalyzer.Analyze(input, tableRules);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Input does not correct: {ex}");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {Delimiter = ";"};
            using var writer = new StreamWriter("table.csv");
            using var csv = new CsvWriter(writer, config);
            csv.WriteHeader<TableRule>();
            csv.NextRecord();
            foreach (var rule in tableRules)
            {
                csv.WriteRecord(rule);
                csv.NextRecord();
            }
        }
    }
}