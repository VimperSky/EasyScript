using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser.Actions
{
    internal class DirSetsFinder
    {
        private readonly List<HashSet<(string Value, bool IsTerm)>> _foundValues = new();
        private readonly List<Rule> _rules;
        private readonly ImmutableHashSet<string> _nonTerms;

        public DirSetsFinder(RuleList ruleList)
        {
            _rules = ruleList.Rules;
            _nonTerms = ruleList.NonTerminals.ToImmutableHashSet();

            for (var i = 0; i < _rules.Count; i++) 
                _foundValues.Add(new HashSet<(string Value, bool IsTerm)>());
        }

        private HashSet<(string Value, bool IsTerm)> FindUp(string nonTerm) => FindUp(nonTerm, new HashSet<int>());
        
        private HashSet<(string Value, bool IsTerm)> FindUp(string nonTerm, HashSet<int> history)
        {
            var returns = new HashSet<(string Value, bool IsTerm)>();
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                for (var j = 0; j < rule.Items.Count; j++)
                {
                    if (rule.Items[j].Value == nonTerm)
                    {
                        if (++j < rule.Items.Count)
                        {
                            returns.Add((rule.Items[j].Value, rule.Items[j].IsTerminal));
                        }
                        else
                        {
                            if (history.Contains(i))
                                return returns;
                            
                            history.Add(i);
                            var nextReturns = FindUp(rule.NonTerminal, history);
                            foreach (var item in nextReturns)
                                returns.Add(item);
                        }
                    }
                }
            }

            return returns;
        }

        public List<DirRule> Find()
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                if (rule.Items[0].Value == "e")
                {
                    foreach (var item in FindUp(rule.NonTerminal)) 
                        _foundValues[i].Add(item);
                }
                else
                {
                    _foundValues[i].Add((rule.Items[0].Value, rule.Items[0].IsTerminal));
                }
            }

            var foundNonTerms = new HashSet<string>();
            foreach (var nonTerm in _nonTerms)
            {
                var allFound = true;
                for (var i = 0; i < _rules.Count; i++)
                {
                    if (_rules[i].NonTerminal != nonTerm)
                        continue;

                    if (_foundValues[i].Any(x => !x.IsTerm))
                    {
                        allFound = false;
                    }
                }

                if (allFound)
                    foundNonTerms.Add(nonTerm);
            }
            
            while (_nonTerms.Count > foundNonTerms.Count + 1)
            {
                foreach (var foundVal in _foundValues)
                {
                    var nonTerms = foundVal.Where(x => !x.IsTerm).ToList();
                    if (nonTerms.Count == 0) 
                        continue;
                    
                    foreach (var nonTerm in nonTerms)
                    {
                        var allFound = true;
                        for (var j = 0; j < _rules.Count; j++)
                        {
                            if (_rules[j].NonTerminal != nonTerm.Value)
                                continue;

                            if (_foundValues[j].Any(x => !x.IsTerm))
                            {
                                allFound = false;
                            }
                        }

                        if (allFound)
                        {
                            foundNonTerms.Add(nonTerm.Value);
                            foundVal.Remove(nonTerm);
                            
                            for (var j = 0; j < _rules.Count; j++)
                            {
                                if (_rules[j].NonTerminal != nonTerm.Value)
                                    continue;

                                foreach (var item in _foundValues[j])
                                    foundVal.Add(item);
                            }

                        }
                    }
                }
            }
            
            return _rules.Select((t, i) => DirRule.Create(_foundValues[i]
                .Select(x => x.Value).ToHashSet(), t)).ToList();
        }
    }
}