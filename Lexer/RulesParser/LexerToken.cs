using System;
using Lexer.Types;

namespace Lexer.RulesParser
{
    public class LexerToken
    {
        public string NonTerminal { get; init; }
        public TokenType? TokenType { get; set; }

        public override string ToString()
        {
            if (NonTerminal == null && TokenType == null)
            {
                throw new ArgumentException("Both NonTerminal and TokenType can't be null!");
            }
            return NonTerminal ?? TokenType.ToString();
        }
    }
}