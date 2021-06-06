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
            // Если сделать по-старому, то от e не избавляемся
            var emptyRemover = new EmptyRemover(rules);
            emptyRemover.RemoveEmpty();
            var newRules = emptyRemover._rules;
            
            new RulesFixer(_rulesProcessor, lettersProvider).FixRules(newRules, true);
            IndexesSetter.SetIndexes(newRules);
            
            foreach (var item in newRules) 
                Console.WriteLine(item);
            Console.WriteLine();
            
            var tableRules = new TableBuilder(newRules).CreateTable();
            
            CsvExport.SaveToCsv(tableRules);

            var input = _inputParser.Parse(); 
            try
            {
                Analyzer.Analyze(input, tableRules, newRules);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}