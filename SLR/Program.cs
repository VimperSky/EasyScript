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
            
            var noEmptyRules = new EmptyRemover(rules).RemoveEmpty();

            var fixedRules = new RulesFixer().FixRules(noEmptyRules);

            foreach (var item in fixedRules) Console.WriteLine(item);
            Console.WriteLine();
            
            var tableRules = new TableBuilder(fixedRules).CreateTable();
            
            CsvExport.SaveToCsv(tableRules);
            
            var input = File.OpenRead("input.txt");
            var analyzer = new Analyzer(input, tableRules, fixedRules);
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