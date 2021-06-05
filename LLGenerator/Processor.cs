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
        
        private static bool IsLLFirst(IEnumerable<DirRule> dirRules)
        {
            var groups = dirRules.GroupBy(x => x.NonTerminal);
            return groups.Select(group => group.SelectMany(x => x.Dirs).ToList())
                .All(groupsDirs => groupsDirs.Count == groupsDirs.Distinct().Count());
        }
        
        public void Process(RulesKeeper rulesKeeper = null, bool buildTable = true)
        {
            var lettersProvider = new LettersProvider();
            var inputRules = _rulesParser.Parse();

            var rules = _rulesProcessor.Process(inputRules);
            var fixedRules = new RulesFixer(lettersProvider).FixRules(rules);
            UpdateLettersProvider(fixedRules, lettersProvider);
            
            var factorizedRules = new Factorization(lettersProvider).MakeFactorization(fixedRules);

            var leftRules = new LeftRecursionRemover(lettersProvider).RemoveLeftRecursion(factorizedRules);
            var dirRules = DirSetsFinder.Find(leftRules);
            if (rulesKeeper != null)
                rulesKeeper.DirRules = dirRules;
            
            Console.WriteLine("Rules:");
            foreach (var rule in dirRules) 
                Console.WriteLine(rule);
            
            if (!IsLLFirst(dirRules))
            {
                Console.WriteLine("Not LL1 grammar");
                return;
            }

            if (!buildTable)
                return;
            
            var table = TableGenerator.TableGenerator.Parse(dirRules);
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