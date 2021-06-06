using System;
using System.IO;
using System.Linq;
using Generator;
using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using SLR.Table;

namespace SLR
{
    public class Processor
    {
        private readonly IInputParser _inputParser;
        private readonly IRulesParser _rulesParser;
        private readonly IRulesProcessor _rulesProcessor;

        public Processor(IRulesParser rulesParser, IRulesProcessor rulesProcessor, IInputParser inputParser)
        {
            _inputParser = inputParser;
            _rulesParser = rulesParser;
            _rulesProcessor = rulesProcessor;
        }

        public void Process()
        {            
            var lettersProvider = new LettersProvider();
            
            var inputRules = _rulesParser.Parse();
            
            var rules = _rulesProcessor.Process(inputRules);
            
            // Применяем к правилам различные фиксы
            new EmptyRemover(rules).RemoveEmpty();
            new RulesFixer(_rulesProcessor, lettersProvider).FixRules(rules, true);
            IndexesSetter.SetIndexes(rules);
            
            foreach (var item in rules) 
                Console.WriteLine(item);
            Console.WriteLine();
            
            var tableRules = new TableBuilder(rules).CreateTable();
            
            CsvExport.SaveToCsv(tableRules);

            var input = _inputParser.Parse(); 
            try
            {
                Analyzer.Analyze(input, tableRules, rules);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}