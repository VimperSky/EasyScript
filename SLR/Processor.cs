using System;
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
            
            var fixedRules = new EmptyRemover(rules).RemoveEmpty();

            // Применяем к правилам различные фиксы
            new RulesFixer(_rulesProcessor, lettersProvider).FixRules(fixedRules, true);
            IndexesSetter.SetIndexes(fixedRules);
            
            foreach (var item in fixedRules) 
                Console.WriteLine(item);
            Console.WriteLine();
            
            var tableRules = new TableBuilder(fixedRules).CreateTable();
            
            CsvExport.SaveToCsv(tableRules);

            var input = _inputParser.Parse(); 
            try
            {
                Analyzer.Analyze(input, tableRules, fixedRules);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}