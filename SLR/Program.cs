using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using SLR;

IRulesParser rulesParser = args.Length > 0 ? new GoogleSheetsRulesParser(args[0]) : new TxtRulesParser("rules.txt");

var processor = new Processor(rulesParser,
    new LexerRulesProcessor(), new LexerInputParser("input.txt"));

processor.Process();