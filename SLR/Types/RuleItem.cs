using System;

namespace SLR.Types
{
    public class RuleItem : IEquatable<RuleItem>
    {
        public readonly string Value;
        public readonly ElementType Type;

        public RuleItem(string value, ElementType type)
        {
            Value = value;
            Type = type;
        }

        
        public int RuleIndex { get; private set; }
        public int ItemIndex { get; private set; }

        public void SetIndex(int ruleId, int itemId)
        {
            RuleIndex = ruleId;
            ItemIndex = itemId;
        }

        public bool Equals(RuleItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Value, other.Value) && Equals(RuleIndex, other.RuleIndex) && Equals(ItemIndex, other.ItemIndex);
        }


        public override string ToString()
        {
            return Value + (RuleIndex + 1) + (ItemIndex + 1);
        }

        public static bool operator ==(RuleItem ruleItem, string value)
        {
            if (!ReferenceEquals(null, ruleItem))
            {
                return ruleItem.Value == value;
            }

            return false;
        }

        public static bool operator !=(RuleItem ruleItem, string value)
        {
            return !(ruleItem == value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RuleItem) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, RuleIndex, ItemIndex);
        }
    }
}