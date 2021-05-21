using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public static class SimpleRulesParser
    {
        public static ImmutableList<Rule> Parse(Stream stream)
        {
            using var sr = new StreamReader(stream);
            string line;
            var rawRules = new List<(string LeftBody, string RightBody)>();
            while ((line = sr.ReadLine()) != null)
            {
                var split = line.Split("->", StringSplitOptions.TrimEntries);
                var localRules = split[1].Split("|", StringSplitOptions.TrimEntries);
                rawRules.AddRange(localRules.Select(rule => (split[0], rule)));
            }

            var nonTerminals = rawRules.Select(x => x.LeftBody).ToHashSet();
            var rules = rawRules.Select(rawRule => new Rule
            {
                NonTerminal = rawRule.LeftBody,
                Items = rawRule.RightBody.Split(" ", StringSplitOptions.TrimEntries)
                    .Select(x => nonTerminals.Contains(x) ? new RuleItem(x) : new RuleItem(x, true)).ToList()
            }).ToList();

            
            if (rules[0].Items[^1] == Constants.EndSymbol 
                && rules.Select(x => x.NonTerminal == rules[0].NonTerminal).Count() > 1)
                rules.Insert(0, new Rule
                {
                    NonTerminal = Extensions.GetNextFreeLetter(rules.GroupBy(x => x.NonTerminal)
                        .Select(k => k.Key).ToHashSet()),
                    Items = new List<RuleItem> {new(rules[0].NonTerminal), new(Constants.EndSymbol, true)}
                });

            return rules.ToImmutableList();
        }
    }
}