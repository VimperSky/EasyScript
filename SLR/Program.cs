using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using SLR;

var sheetName = args.Length > 0 ? args[0] : "Ready";

var processor = new Processor(new GoogleSheetsRulesParser(sheetName),
    new LexerRulesProcessor(), new LexerInputParser("input.txt"));

processor.Process();