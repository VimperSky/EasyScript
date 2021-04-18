using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SetsParser
{
    public class SetsParser
    {
        public SetsParser(Stream input)
        {
            var baseRules = ParseInput(input);
            var factorized = Factorization(baseRules);
            var finalRules = RemoveLeftRecursion(factorized);
            var firstList = TransitiveClosure(finalRules);

            for (var i = 0; i < finalRules.Count; i++)
            {
                Console.WriteLine($"{finalRules[i]}  [{firstList[i]}]");
            }
        }

        private static List<string> TransitiveClosure(List<Rule> rules)
        {
            var listOfFirsts = new List<string>();
            foreach (var rule in rules)
                listOfFirsts.Add(rule.Items[0].IsTerminal
                    ? rule.Items[0].Value
                    : RecursionSearch(rules, rule.Items[0].Value));

            return listOfFirsts;
        }

        private static string RecursionSearch(List<Rule> rules, string wanted)
        {
            foreach (var x in rules)
                if (wanted == x.NonTerminal && x.Items[0].IsTerminal)
                    return x.Items[0].Value;
                else if (wanted == x.NonTerminal) 
                    return RecursionSearch(rules, wanted);

            return null;
        }

        private static List<Rule> RemoveLeftRecursion(RulesTable rulesTable)
        {
            var newRules = new List<Rule>();
            foreach (var t in rulesTable.NonTerminals.ToList())
            {
                var rules = rulesTable.Rules.Where(x => x.NonTerminal == t).ToList();
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
                        if (nonSimilarRules.Count == 0) throw new Exception("Infinity recursion");
                        var newNonTerm = Extensions.GetNextFreeLetter(rulesTable.NonTerminals).ToString();
                        nonSimilarRules[0].Items.Add(new RuleItem(newNonTerm, false));
                        var newRule = new Rule
                        {
                            NonTerminal = t,
                            Items = nonSimilarRules[0].Items
                        };
                        newRules.Add(newRule);
                        newRules.Add(new Rule
                        {
                            NonTerminal = newNonTerm, Items = new List<RuleItem>
                            {
                                new("e", true)
                            }
                        });
                        foreach (var r in similarRules)
                        {
                            var rest = r.Items.Skip(1).ToList();
                            rest.Add(new RuleItem(newNonTerm, false));
                            newRules.Add(new Rule {NonTerminal = newNonTerm, Items = rest});
                        }

                        continue;
                    }
                }

                newRules.AddRange(rules);
            }

            return newRules;
        }

        private static RulesTable Factorization(RulesTable rulesTable)
        {
            var newRules = new List<Rule>();
            var newNonTerminals = rulesTable.NonTerminals;
            foreach (var t in rulesTable.NonTerminals.ToList())
            {
                var rules = rulesTable.Rules.Where(x => x.NonTerminal == t).ToList();
                if (rules.Count > 1)
                {
                    var common = rules.FindCommon();
                    if (common.Count > 0)
                    {
                        var newNonTerm = Extensions.GetNextFreeLetter(rulesTable.NonTerminals).ToString();
                        common.Add(new RuleItem(newNonTerm, false));
                        var newRule = new Rule
                        {
                            NonTerminal = t,
                            Items = common
                        };
                        newRules.Add(newRule);
                        newRules.Add(new Rule
                        {
                            NonTerminal = newNonTerm, Items = new List<RuleItem>
                            {
                                new("e", true)
                            }
                        });
                        newNonTerminals.Add(newNonTerm);
                        foreach (var rule in rules)
                        {
                            var rest = rule.Items.Skip(common.Count - 1).ToList();
                            if (rest.Count == 0)
                                continue;
                            newRules.Add(new Rule {NonTerminal = newNonTerm, Items = rest});
                        }

                        continue;
                    }
                }

                newRules.AddRange(rules);
            }

            return new RulesTable(newRules, newNonTerminals);
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

            return new RulesTable(rules, nonTerminals);
        }
    }
}