using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;

namespace Generator.RulesProcessing
{
    public class SimpleRulesProcessor: IRulesProcessor
    {
        public ImmutableList<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules)
        {
            var nonTerminals = inputRules.Select(x => x.NonTerminal).ToHashSet();

            var rules = inputRules.Select(rawRule => new Rule
                {
                    NonTerminal = rawRule.NonTerminal,
                    Items = rawRule.RightBody.Split(" ", StringSplitOptions.TrimEntries)
                        .Select(x => nonTerminals.Contains(x)
                            ? new RuleItem(x, ElementType.NonTerminal)
                            : x == Constants.EmptySymbol
                                ? new RuleItem(x, ElementType.Empty)
                                : x == Constants.EndSymbol
                                    ? new RuleItem(x, ElementType.End)
                                    : new RuleItem(x, ElementType.Terminal))
                        .ToList()
                })
                .ToList();
            

            return rules.ToImmutableList();
        }
    }
}