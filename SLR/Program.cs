using System;
using System.IO;
using SLR.Table;

namespace SLR
{
    internal static class Program
    {
        private static void Main()
        {
            var rules = new SimpleRulesParser().Parse(File.OpenRead("rules.txt"));
            
            var noEmptyRules = new EmptyRemover().RemoveEmpty(rules);
            
            var tableRules = new TableBuilder(noEmptyRules).CreateTable();
            
            CsvExport.SaveToCsv(tableRules);
            
            var input = File.OpenRead("input.txt");
            var analyzer = new Analyzer(input, tableRules, rules);
            try
            {
                analyzer.Analyze();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}