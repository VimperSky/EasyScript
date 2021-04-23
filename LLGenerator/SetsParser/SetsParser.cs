using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LLGenerator.Entities;
using LLGenerator.SetsParser.Actions;

namespace LLGenerator.SetsParser
{
    public static class SetsParser
    {
        public static List<DirRule> DoParse(Stream input)
        {
            var baseRules = ParseInput(input);
            var leftRules = LeftRecursionRemover.RemoveLeftRecursion(baseRules);
            var factorizedRules = Factorization.MakeFactorization(leftRules);
            var dirRules = new DirSetsFinder(factorizedRules).Find();

            return dirRules;
        }

        private static RuleList ParseInput(Stream input)
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
                        .Select(x => new RuleItem(x, !nonTerminals.Contains(x)))
                        .ToList()
                })
                .ToList();

            if (rules[0].Items[^1].Value != Constants.NewLineSymbol)
                rules[0].Items.Add(new RuleItem(Constants.NewLineSymbol, true));

            return new RuleList(rules, nonTerminals);
        }
    }
}