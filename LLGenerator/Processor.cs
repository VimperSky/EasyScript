using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator;
using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using Generator.Types;
using LLGenerator.SetsParser;
using LLGenerator.TableGenerator;
using LLGenerator.Types;

namespace LLGenerator
{
    public class Processor
    {
        private readonly IRulesParser _rulesParser;
        private readonly IRulesProcessor _rulesProcessor;
        private readonly IInputParser _inputParser;
        public Processor(IRulesParser rulesParser, IRulesProcessor rulesProcessor, IInputParser inputParser)
        {
            _rulesParser = rulesParser;
            _rulesProcessor = rulesProcessor;
            _inputParser = inputParser;
        }
        
        // ReSharper disable once InconsistentNaming
        private static List<string> FindNotLLGroups(IEnumerable<DirRule> dirRules)
        {
            var duplicates = new List<string>();
            var groups = dirRules.GroupBy(x => x.NonTerminal);
            foreach (var group in groups)
            {
                var dirs = group.SelectMany(x => x.Dirs).ToList();
                if (dirs.Count != dirs.Distinct().Count())
                {
                    duplicates.Add(group.Key);
                }
            }

            return duplicates;
        }
        

        public ImmutableList<DirRule> GenerateRules()
        {
            var lettersProvider = new LettersProvider();
            var inputRules = _rulesParser.Parse();

            var rules = _rulesProcessor.Process(inputRules);
            var fixedRules = new RulesFixer(_rulesProcessor, lettersProvider).FixRules(rules);
            UpdateLettersProvider(fixedRules, lettersProvider);
            
            var factorizedRules = new Factorization(lettersProvider).MakeFactorization(fixedRules);

            var leftRules = new LeftRecursionRemover(lettersProvider).RemoveLeftRecursion(factorizedRules);
            var dirRules = DirSetsFinder.Find(leftRules);
            
            Console.WriteLine("Rules:");
            foreach (var rule in dirRules) 
                Console.WriteLine(rule);

            return dirRules;
        }
        
        public void Process()
        {
            var dirRules = GenerateRules();

            var notLLGroups = FindNotLLGroups(dirRules);
            if (notLLGroups.Count > 0)
            {
                Console.WriteLine("Not LL1 grammar");
                foreach (var item in notLLGroups)
                    Console.WriteLine(item);
                return;
            }
            
            var table = TableBuilder.Build(dirRules);
            CsvExport.SaveToCsv(table);

            var input = _inputParser.Parse();
            
            ImmutableList<int> history;
            try
            {
                history = SyntaxAnalyzer.SyntaxAnalyzer.Analyze(input, table);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Correct! History: [{string.Join(", ", history)}]");
        }
        
        private static void UpdateLettersProvider(ImmutableList<Rule> rules, LettersProvider lettersProvider)
        {
            foreach (var letter in rules.Select(x => x.NonTerminal).Where(x => x.Length == 1))
                lettersProvider.TakeLetter(letter[0]);
        }

    }
}