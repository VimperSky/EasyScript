using System.Collections.Generic;

namespace SetsParser
{
    public class DirSetsFinder
    {
        private readonly List<HashSet<string>> _foundValues = new();
        private readonly List<Rule> _rules;

        public DirSetsFinder(List<Rule> rules)
        {
            _rules = rules;

            for (var i = 0; i < _rules.Count; i++)
            {
                _foundValues.Add(new HashSet<string>());
            }
        }

        public List<HashSet<string>> Find()
        {
            for (var index = 0; index < _rules.Count; index++)
            {
                FindN(index, index);
            }

            return _foundValues;
        }

        private void FindN(int globalIndex, int origRuleId, int localIndex = 0)
        {
            var rule = _rules[globalIndex];
            if (rule.Items[localIndex].IsTerminal)
            {
                if (rule.Items[localIndex].Value == "e")
                {
                    FindUp(rule.NonTerminal, origRuleId);
                }
                else
                {
                    _foundValues[origRuleId].Add(rule.Items[localIndex].Value);
                }
            }
            else
            {
                for (var index = 0; index < _rules.Count; index++)
                {
                    if (_rules[index].NonTerminal == rule.Items[localIndex].Value)
                    {
                        FindN(index, origRuleId);
                    }
                }            
            }
        }

        private void FindUp(string nonTerm, int origRuleId, List<string> recursiveNonTerms = null)
        {
            for (var globalIndex = 0; globalIndex < _rules.Count; globalIndex++)
            {
                var rule = _rules[globalIndex];
                for (var localIndex = 0; localIndex < rule.Items.Count; localIndex++)
                {
                    if (rule.Items[localIndex].Value == nonTerm)
                    {
                        if (++localIndex < rule.Items.Count)
                        {
                            FindN(globalIndex, origRuleId, localIndex);
                        }
                        else
                        {
                            recursiveNonTerms ??= new List<string>();
                            if (recursiveNonTerms.Contains(rule.NonTerminal))
                                continue;
                            
                            recursiveNonTerms.Add(rule.NonTerminal);
                            
                            FindUp(rule.NonTerminal, origRuleId, recursiveNonTerms);
                        }
                    }
                }
            }
        }
    }
}