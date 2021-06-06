using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;

namespace Generator.RulesProcessing
{
    public class SimpleRulesProcessor: IRulesProcessor
    {
        private HashSet<string> _nonTerminals;
        
        public RuleItem ParseToken(string token)
        {
            return _nonTerminals.Contains(token)
                ? new RuleItem(token, ElementType.NonTerminal)
                : token == Constants.EmptySymbol
                    ? new RuleItem(token, ElementType.Empty)
                    : token == Constants.EndSymbol
                        ? new RuleItem(token, ElementType.End)
                        : new RuleItem(token, ElementType.Terminal);
        }

        public ImmutableList<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules)
        {
            _nonTerminals = inputRules.Select(x => x.NonTerminal).ToHashSet();

            var rules = inputRules.Select(rawRule => new Rule
                {
                    NonTerminal = rawRule.NonTerminal,
                    Items = rawRule.RightBody.Split(" ", StringSplitOptions.TrimEntries)
                        .Select(ParseToken).ToList()
                }).ToList();

            return rules.ToImmutableList();
        }
    }
}