#nullable enable
using System;

namespace SetsParser
{
    internal class RuleItem : IEquatable<RuleItem>
    {
        public readonly bool IsTerminal;
        public readonly string Value;

        public RuleItem(string value, bool isTerminal)
        {
            Value = value;
            IsTerminal = isTerminal;
        }


        public bool Equals(RuleItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value && IsTerminal == other.IsTerminal;
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