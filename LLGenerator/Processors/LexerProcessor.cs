using System.IO;
using Generator.InputParsing;
using Generator.RulesParsing;
using Generator.RulesProcessing;

namespace LLGenerator.Processors
{
    public class LexerProcessor: Processor
    {
        protected override Stream RulesStream => File.OpenRead("rules.csv");
        protected override Stream InputStream => File.OpenRead("input.txt");
        
        public LexerProcessor() : base(new CsvRulesParser(), new LexerRulesProcessor(), new LexerInputProcessor())
        {
            
        }
    }
}