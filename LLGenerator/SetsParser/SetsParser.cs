using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser
{
    public static class SetsParser
    {
        public static List<DirRule> DoParse(Stream input)
        {
            var baseRules = ParseInput(input);
            var noLeftRules = RemoveLeftRecursion(baseRules);
            var factorizedRules = Factorization(noLeftRules);
            var dirRules = new DirSetsFinder(factorizedRules.Rules).Find();

            return dirRules;
        }

        private static RulesTable RemoveLeftRecursion(RulesTable rulesTable)
        {
            var newRules = new List<Rule>();
            var oldRules = rulesTable.Rules.ToList();
            var nonTerms = rulesTable.NonTerminals;

            while (oldRules.Count > 0)
            {
                var nonTerminal = oldRules[0].NonTerminal;
                var rules = rulesTable.Rules.Where(x => x.NonTerminal == nonTerminal).ToList();
                oldRules.RemoveRange(0, rules.Count);
                if (rules.Count > 1)
                {
                    var similarRules = new List<Rule>();
                    var nonSimilarRules = new List<Rule>();
                    foreach (var rule in rules)
                        if (rule.Items[0].Value == rule.NonTerminal)
                            similarRules.Add(rule);
                        else
                            nonSimilarRules.Add(rule);

                    if (similarRules.Count > 0)
                    {
                        if (nonSimilarRules.Count == 0)
                            throw new Exception("Infinity recursion");

                        var newNonTerm = SetsParserExtensions.GetNextFreeLetter(nonTerms).ToString();
                        nonTerms.Add(newNonTerm);
                        foreach (var r in nonSimilarRules)
                        {
                            r.Items.Add(new RuleItem(newNonTerm, false));
                            newRules.Add(r);
                        }

                        foreach (var r in similarRules)
                        {
                            var rest = r.Items.Skip(1).ToList();
                            rest.Add(new RuleItem(newNonTerm, false));
                            newRules.Add(new Rule {NonTerminal = newNonTerm, Items = rest});
                        }

                        newRules.Add(new Rule
                        {
                            NonTerminal = newNonTerm, Items = new List<RuleItem>
                            {
                                new("e", true)
                            }
                        });

                        continue;
                    }
                }

                newRules.AddRange(rules);
            }

            return new RulesTable(newRules, nonTerms);
        }

        private static RulesTable Factorization(RulesTable rulesTable)
        {
            var newRules = new List<Rule>();
            var oldRules = rulesTable.Rules.ToList();

            var nonTerminals = rulesTable.NonTerminals;
            while (oldRules.Count > 0)
            {
                var nonTerminal = oldRules[0].NonTerminal;
                var rules = rulesTable.Rules.Where(x => x.NonTerminal == nonTerminal).ToList();
                oldRules.RemoveRange(0, rules.Count);

                if (rules.Count > 1)
                    for (;;)
                    {
                        var minCommonLen = int.MaxValue;
                        var commonIds = new List<int> {0};
                        for (var i = 1; i < rules.Count; i++)
                        {
                            var common = rules[0].FindCommon(rules[i]);
                            if (common.Count == 0)
                                continue;

                            if (common.Count < minCommonLen)
                                minCommonLen = common.Count;

                            commonIds.Add(i);
                        }

                        if (commonIds.Count == 1)
                            break;

                        var newNonTerm = SetsParserExtensions.GetNextFreeLetter(nonTerminals).ToString();
                        nonTerminals.Add(newNonTerm);
                        var newItems = rules[0].Items.Take(minCommonLen).ToList();
                        newItems.Add(new RuleItem(newNonTerm, false));
                        newRules.Add(new Rule
                        {
                            NonTerminal = nonTerminal,
                            Items = newItems
                        });
                        foreach (var index in commonIds)
                        {
                            var truncating  = rules[index].Items.Skip(minCommonLen).ToList();
                            newRules.Add(new Rule
                            {
                                NonTerminal = newNonTerm,
                                Items = truncating.Count == 0 ? new List<RuleItem> {new("e", true)} : truncating
                            });
                        }

                        foreach (var index in commonIds.OrderByDescending(v => v))
                            rules.RemoveAt(index);
                    }

                newRules.AddRange(rules);
            }

            return new RulesTable(newRules, nonTerminals);
        }

        private static RulesTable ParseInput(Stream input)
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

            return new RulesTable(rules, nonTerminals);
        }
    }
}