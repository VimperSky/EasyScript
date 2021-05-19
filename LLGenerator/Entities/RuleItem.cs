using System;
using Lexer.Types;

namespace LLGenerator.Entities
{
    public class RuleItem
    {
        public string NonTerminal { get; }
        public TokenType? TokenType { get;}

        public string Value => ToString();

        public bool IsTerminal => NonTerminal == null;

        public RuleItem(string value)
        {
            NonTerminal = value;
        }

        public RuleItem(TokenType value)
        {
            TokenType = value;
        }
        
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