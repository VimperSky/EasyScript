using System;
using System.IO;

namespace SLR
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var rules = SimpleRulesParser.Parse(File.OpenRead("rules.txt"));

            var tableBuilder = new TableBuilder(rules);
            var tableRules = tableBuilder.CreateTable();

            var input = File.OpenRead("input.txt");
            var analyzer = new Analyzer(input, tableRules, rules);
            try
            {
                analyzer.Analyze();
                Console.WriteLine("Analyzer correct!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}