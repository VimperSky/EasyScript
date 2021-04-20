using System;
using System.Globalization;
using System.IO;
using Common;
using CsvHelper;
using CsvHelper.Configuration;
using LLTableGenerator;

namespace TestSolver
{
    internal static class Program
    {
        private static void Main()
        {
            var inputStream = File.OpenRead("input.txt");
            var dirRules = SetsParser.SetsParser.DoParse(inputStream);
            foreach (var rule in dirRules)
                Console.WriteLine(rule);


            var tableRules = Generator.Parse(dirRules);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {Delimiter = " "};
            using (var writer = new StreamWriter("table.csv"))
            {
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteHeader<TableRule>();
                    csv.NextRecord();
                    foreach (var rule in tableRules)
                    {
                        csv.WriteRecord(rule);
                        csv.NextRecord();
                    }
                }
            }

            foreach (var tableRule in tableRules)
                Console.WriteLine(tableRule);
        }
    }
}