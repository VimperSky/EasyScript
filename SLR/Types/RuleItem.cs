﻿using System;

namespace SLR.Types
{
    public class RuleItem: IEquatable<RuleItem>
    {
        private readonly string _nonTerminal;
        private readonly string _terminal;

        public RuleItem(string value, bool isTerminal = false)
        {
            if (isTerminal)
            {
                _terminal = value;
                _nonTerminal = null;
            }
            else
            {
                _terminal = null;
                _nonTerminal = value;
            }
        }

        
        public string Value => _nonTerminal ?? (_terminal ??
                                                throw new Exception("Both NonTerminal and Terminal can't be null!"));

        public RuleItemId Id { get; set; }
        

        public override string ToString()
        {
            return Value + Id;
        }
        
        public static bool operator ==(RuleItem ruleItem, string value)
        { 
            if (!ReferenceEquals(null, ruleItem))
            {
                if (ruleItem._nonTerminal != null)
                    return ruleItem._nonTerminal == value;

                if (ruleItem._terminal != null)
                    return ruleItem._terminal == value;
            }

            return false;
        }

        public static bool operator !=(RuleItem ruleItem, string value)
        {
            return !(ruleItem == value);
        }

        public bool Equals(RuleItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _nonTerminal.Equals(other._nonTerminal) && _terminal.Equals(other._terminal) && Id.Equals(other.Id);
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
            return HashCode.Combine(_nonTerminal, _terminal, Id);
        }
    }
}