﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public class EmptyRemover
    {
        private readonly List<Rule> _rules;
        public EmptyRemover(ImmutableList<Rule> rules)
        {
            _rules = rules.ToList();
        }
        
        public ImmutableList<Rule> RemoveEmpty()
        {
            var foundEmpty = true;
            while (foundEmpty)
            {
                foundEmpty = RemoveEmptySingle();
            }
            return _rules.ToImmutableList();
        }

        private bool RemoveEmptySingle()
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                for (var j = 0; j < _rules[i].Items.Count; j++)
                {
                    if (_rules[i].Items[j].Type is ElementType.Empty)
                    {
                        var nonTerminal = _rules[i].NonTerminal;
                        _rules.RemoveAt(i);
                        RebuildRules(nonTerminal);
                        return true;
                    }
                }
            }

            return false;
        }
        
        private void RebuildRules(string nonTerm)
        {
            foreach (var rule in _rules.ToList())
            {
                if (rule.Items.Any(x => x.Value == nonTerm))
                {
                    var newItems = rule.Items
                        .Where(x => x.Value != nonTerm)
                        .Select(x => x.Clone()).ToList();
                    if (newItems.All(x => x.Type is ElementType.End))
                        continue;
                    
                    var newRule = new Rule {NonTerminal = rule.NonTerminal, Items = newItems};
                    var index = _rules.FindLastIndex(x => x.NonTerminal == newRule.NonTerminal);
                    _rules.Insert(index + 1, newRule);
                }
            }
        }
    }
}