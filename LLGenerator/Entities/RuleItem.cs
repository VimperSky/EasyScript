using System;
using Lexer.Types;

namespace LLGenerator.Entities
{
    public class RuleItem : IEquatable<RuleItem>
    {
        public RuleItem(string value)
        {
            NonTerminal = value;
        }

        public RuleItem(TokenType value)
        {
            TokenType = value;
        }

        private string NonTerminal { get; }
        public TokenType? TokenType { get; }

        public string Value => ToString();

        public bool IsTerminal => NonTerminal == null;

        public bool Equals(RuleItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value && IsTerminal == other.IsTerminal;
        }

        public override string ToString()
        {
            if (NonTerminal == null && TokenType == null)
                throw new ArgumentException("Both NonTerminal and TokenType can't be null!");
            return NonTerminal ?? TokenType.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RuleItem) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, IsTerminal);
        }
    }
}