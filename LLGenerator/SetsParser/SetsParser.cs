using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using LLGenerator.Entities;
using LLGenerator.SetsParser.Actions;

namespace LLGenerator.SetsParser
{
    public static class SetsParser
    {
        public static ImmutableList<DirRule> DoParse(Stream input)
        {
            var baseRules = ParseInput(input);
            var factorizedRules = Factorization.MakeFactorization(baseRules);
            var leftRules = LeftRecursionRemover.RemoveLeftRecursion(factorizedRules);
            var dirRules = new DirSetsFinder(leftRules).Find();
            return dirRules;
        }

        public static bool IsLLFirst(IEnumerable<DirRule> dirRules)
        {
            var groups = dirRules.GroupBy(x => x.NonTerminal);
            return groups.Select(group => group.SelectMany(x => x.Dirs).ToList())
                .All(groupsDirs => groupsDirs.Count == groupsDirs.Distinct().Count());
        }

        private static ImmutableList<Rule> ParseInput(Stream input)
        {
            using var sr = new StreamReader(input);
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
                    .Select(x => new RuleItem(x, !nonTerminals.Contains(x))).ToList()
            }).ToList();

            if (rules[0].Items[^1].Value != Constants.EndSymbol &&
                rules.Select(x => x.NonTerminal == rules[0].NonTerminal).Count() > 1)
                rules.Insert(0, new Rule
                {
                    NonTerminal = SetsParserExtensions.GetNextFreeLetter(rules.GroupBy(x => x.NonTerminal)
                        .Select(k => k.Key).ToHashSet()),
                    Items = new List<RuleItem> {new(rules[0].NonTerminal, false), new(Constants.EndSymbol, true)}
                });

            return rules.ToImmutableList();
        }
    }
}