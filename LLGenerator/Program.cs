using LLGenerator.Processors;

namespace LLGenerator
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var isLexerMode = args.Length > 0;
            Processor processor = isLexerMode ? new LexerProcessor() : new SimpleProcessor();
            processor.Process();
        }
    }
}