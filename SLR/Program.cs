using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using SLR;

var isLexerMode = args.Length > 0;
Processor processor;
if (isLexerMode)
    processor = new Processor(new CsvRulesParser("rules.csv"),
        new LexerRulesProcessor(), new LexerInputParser("input.txt"));
else
    processor = new Processor(new TxtRulesParser("rules.txt"),
        new SimpleRulesProcessor(), new SimpleInputParser("input.txt"));

processor.Process();