using System.IO;
using Generator.RulesParsing;
using Generator.RulesProcessing;

namespace LLGenerator.Processors
{
    public class SimpleProcessor: Processor
    {
        protected override Stream RulesStream => File.OpenRead("rules.txt");
        protected override Stream InputStream => File.OpenRead("input.txt");
        
        
        public SimpleProcessor() : base(new TxtRulesParser(), new SimpleRulesProcessor(), new Generator.InputParsing.SimpleRulesProcessor())
        {
        }
    }
}