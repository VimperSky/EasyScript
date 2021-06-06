﻿using System;
using Generator.Types;

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

        private bool _indexesSet;

        public void SetIndex(int ruleId, int itemId)
        {
            RuleIndex = ruleId;
            ItemIndex = itemId;
            _indexesSet = true;
        }

        public bool Equals(RuleItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Value, other.Value) && Equals(RuleIndex, other.RuleIndex) && Equals(ItemIndex, other.ItemIndex);
        }

        public override string ToString()
        {
            if (Type == ElementType.Collapse)
                return Value;
            
            if (_indexesSet)
                return Value + (RuleIndex + 1) + (ItemIndex + 1);

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