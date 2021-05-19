using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using LLGenerator.Entities;
using LLGenerator.SetsParser;
using LLGenerator.TableGenerator;

namespace LLGenerator
{
    public static class Program
    {
        private static ImmutableList<DirRule> DoParse(Stream input)
        {
            var inputRules = CsvImport.Parse(input);
            var baseRules = LexerRulesParser.Parse(inputRules);
            var factorizedRules = Factorization.MakeFactorization(baseRules);
            var leftRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var dirRules = new DirSetsFinder(leftRules).Find();
            return dirRules;
        }

        private static bool IsLLFirst(IEnumerable<DirRule> dirRules)
        {
            var groups = dirRules.GroupBy(x => x.NonTerminal);
            return groups.Select(group => group.SelectMany(x => x.Dirs).ToList())
                .All(groupsDirs => groupsDirs.Count == groupsDirs.Distinct().Count());
        }
        
        private static void Main()
        {
            var rulesStream = File.OpenRead("rules.csv");
            var dirRules = DoParse(rulesStream);
            Console.WriteLine("Rules:");
            foreach (var rule in dirRules) Console.WriteLine(rule);
            if (!IsLLFirst(dirRules))
            {
                Console.WriteLine("Not LL1 grammar");
                return;
            }

            var tableRules = TableGenerator.TableGenerator.Parse(dirRules);
            CsvExport.SaveToCsv(tableRules);
            var input = File.ReadAllText("input.txt").Split(" ", StringSplitOptions.TrimEntries);
            Console.WriteLine($"Input: {string.Join(" ", input)}");
            ImmutableList<int> history;
            try
            {
                history = SyntaxAnalyzer.SyntaxAnalyzer.Analyze(input, tableRules);
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