using System;
using System.IO;
using Lexer.RulesParser;

namespace Lexer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var lexerRulesParser = new LexerRulesParser();
            var lexRules = lexerRulesParser.Parse();
            
            var lexer = new Lexer(File.OpenRead("input.txt"));
            foreach (var token in lexer.Tokens) Console.WriteLine(token);
        }
    }
}