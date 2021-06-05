using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Generator;
using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using Generator.Types;
using LLGenerator.SetsParser;
using LLGenerator.TableGenerator;
using LLGenerator.Types;

namespace LLGenerator.Processors
{
    public abstract class Processor
    {
        private readonly IRulesParser _rulesParser;
        private readonly IRulesProcessor _rulesProcessor;
        private readonly IInputProcessor _inputProcessor;
        protected Processor(IRulesParser rulesParser, IRulesProcessor rulesProcessor, IInputProcessor inputProcessor)
        {
            _rulesParser = rulesParser;
            _rulesProcessor = rulesProcessor;
            _inputProcessor = inputProcessor;
        }
        
        private static bool IsLLFirst(IEnumerable<DirRule> dirRules)
        {
            var groups = dirRules.GroupBy(x => x.NonTerminal);
            return groups.Select(group => group.SelectMany(x => x.Dirs).ToList())
                .All(groupsDirs => groupsDirs.Count == groupsDirs.Distinct().Count());
        }
        
        public void Process()
        {
            var inputRules = _rulesParser.Parse(RulesStream);

            var rules = _rulesProcessor.Process(inputRules);
            UpdateLettersProvider(rules, LettersProvider.Instance);
            
            var factorizedRules = Factorization.MakeFactorization(rules);

            var leftRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var dirRules = DirSetsFinder.Find(leftRules);
            
            Console.WriteLine("Rules:");
            foreach (var rule in dirRules) 
                Console.WriteLine(rule);
            
            if (!IsLLFirst(dirRules))
            {
                Console.WriteLine("Not LL1 grammar");
                return;
            }

            var table = TableGenerator.TableGenerator.Parse(dirRules);
            CsvExport.SaveToCsv(table);

            var input = _inputProcessor.Parse(InputStream);
            
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

        protected abstract Stream RulesStream { get; }

        protected abstract Stream InputStream { get; }

        private static void UpdateLettersProvider(ImmutableList<Rule> rules, LettersProvider lettersProvider)
        {
            foreach (var letter in rules.Select(x => x.NonTerminal).Where(x => x.Length == 1))
                lettersProvider.TakeLetter(letter[0]);
        }

    }
}