using System;

namespace SLR.Types
{
    public class RuleItemId: IEquatable<RuleItemId>
    {
        public readonly int RuleIndex;
        public readonly int ItemIndex;

        public RuleItemId(int ruleRuleIndex, int ruleItemIndex)
        {
            RuleIndex = ruleRuleIndex;
            ItemIndex = ruleItemIndex;
        }

        public override string ToString()
        {
            return $"{RuleIndex+1}{ItemIndex+1}";
        }

        public bool Equals(RuleItemId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return RuleIndex == other.RuleIndex && ItemIndex == other.ItemIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RuleItemId) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RuleIndex, ItemIndex);
        }
    }
}