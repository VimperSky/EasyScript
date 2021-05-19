using System.Collections.Generic;

namespace Lexer.RulesParser
{
    public class LexerRule
    {
        public string NonTerminal { get; set; }
        public List<LexerToken> Tokens { get; set; }

        public override string ToString()
        {
            return $"{NonTerminal} -> {string.Join(" ", Tokens)}";
        }
    }
}