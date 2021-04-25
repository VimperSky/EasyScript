using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser.Actions
{
    internal class DirSetsFinder
    {
        private readonly List<HashSet<(string Value, bool IsTerm)>> _foundValues = new();
        private readonly ImmutableList<Rule> _rules;

        public DirSetsFinder(ImmutableList<Rule> ruleList)
        {
            _rules = ruleList;
            for (var i = 0; i < _rules.Count; i++)
                _foundValues.Add(new HashSet<(string Value, bool IsTerm)>());
        }

        private IEnumerable<(string Value, bool IsTerm)> FindUp(string nonTerm)
        {
            return FindUp(nonTerm, new HashSet<int>());
        }

        private IEnumerable<(string Value, bool IsTerm)> FindUp(string nonTerm, HashSet<int> history)
        {
            var returns = new HashSet<(string Value, bool IsTerm)>();
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                for (var j = 0; j < rule.Items.Count; j++)
                    if (rule.Items[j].Value == nonTerm)
                    {
                        if (++j < rule.Items.Count)
                        {
                            returns.Add((rule.Items[j].Value, rule.Items[j].IsTerminal));
                        }
                        else
                        {
                            if (history.Contains(i)) return returns;
                            history.Add(i);
                            var nextReturns = FindUp(rule.NonTerminal, history);
                            foreach (var item in nextReturns)
                                returns.Add(item);
                        }
                    }
            }

            return returns;
        }

        public ImmutableList<DirRule> Find()
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                if (rule.Items[0].Value == Constants.EmptySymbol)
                    foreach (var item in FindUp(rule.NonTerminal))
                        _foundValues[i].Add(item);
                else
                    _foundValues[i].Add((rule.Items[0].Value, rule.Items[0].IsTerminal));
            }

            for (;;)
            {
                var somethingChanged = false;
                foreach (var foundVal in _foundValues)
                {
                    var nonTerms = foundVal.Where(x => !x.IsTerm).ToList();
                    if (nonTerms.Count > 0)
                        somethingChanged = true;
                    foreach (var nonTerm in nonTerms)
                    {
                        foundVal.Remove(nonTerm);
                        var rules = _rules.Select((x, i) => (x, i))
                            .Where(x => x.x.NonTerminal == nonTerm.Value)
                            .Select(x => x.i)
                            .ToList();
                        foreach (var fVal in rules.SelectMany(rule => _foundValues[rule]))
                            foundVal.Add(fVal);
                    }
                }

                if (!somethingChanged) break;
            }

            return _rules.Select((t, i) => DirRule.Create(_foundValues[i]
                .Select(x => x.Value).ToHashSet(), t)).ToImmutableList();
        }
    }
}