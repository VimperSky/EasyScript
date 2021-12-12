using System;
using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;
using SLR;
using static System.Enum;

var parsingMode = ParsingMode.LexerOnline;
try
{
    TryParse(args[0], out parsingMode);
}
catch
{
    Console.WriteLine("Не удалось прочитать аргументы командной строки. Используется стандартный режим.");
}

IRulesParser rulesParser = parsingMode switch
{
    ParsingMode.Txt => new TxtRulesParser("rules.txt"),
    ParsingMode.LexerManual => new CsvRulesParser("rules.csv"),
    ParsingMode.LexerOnline => new OnlineCsvRulesParser(),
    _ => throw new ArgumentOutOfRangeException()
};

var processor = new Processor(rulesParser,
    new LexerRulesProcessor(), new LexerInputParser("input.txt"));
        

processor.Process();