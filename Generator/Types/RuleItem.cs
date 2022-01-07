using System;

namespace Generator.Types
{
    public class RuleItem : IEquatable<RuleItem>
    {
        public readonly ElementType Type;
        public readonly string Value;
        public string Action;

        private bool _indexesSet;

        public RuleItem(string value, ElementType type, string action = null)
        {
            Value = value;
            Type = type;
            Action = action;
        }


        public int RuleIndex { get; private set; }
        public int ItemIndex { get; private set; }

        public bool Equals(RuleItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Value, other.Value) && Equals(RuleIndex, other.RuleIndex) &&
                   Equals(ItemIndex, other.ItemIndex);
        }

        public void SetIndex(int ruleId, int itemId)
        {
            RuleIndex = ruleId;
            ItemIndex = itemId;
            _indexesSet = true;
        }

        public override string ToString()
        {
            if (Type == ElementType.Collapse)
            {
                var val =  Value;
                
                if (Action != null)
                    val += $"#{Action}";
                
                return val;
            }

            if (_indexesSet)
                return Value + (RuleIndex + 1) + (ItemIndex + 1) ;
            
            return Value;
        }

        public RuleItem Clone()
        {
            return new(Value, Type);
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