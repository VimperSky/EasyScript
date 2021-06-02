﻿using System;
using System.IO;
using SLR.Table;

namespace SLR
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var rules = SimpleRulesParser.Parse(File.OpenRead("rules.txt"));

            var tableBuilder = new TableBuilder(rules);
            var tableRules = tableBuilder.CreateTable();
            CsvExport.SaveToCsv(tableRules);

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