using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace SetsParser
{
    public static class SetsParser
    {
        public static List<DirRule> DoParse(Stream input)
        {
            var baseRules = ParseInput(input);
            
            var factorizedRules = Factorization(baseRules);
            
            var noLeftRules = RemoveLeftRecursion(factorizedRules);
            
            var dirRules = new DirSetsFinder(noLeftRules).Find();
            return dirRules;
        }
        
        private static List<Rule> RemoveLeftRecursion(RulesTable rulesTable)
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
                        
                        var newNonTerm = Extensions.GetNextFreeLetter(nonTerms).ToString();
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

            return newRules;
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
                {
                    var common = rules.FindCommon();
                    if (common.Count > 0)
                    {
                        var newNonTerm = Extensions.GetNextFreeLetter(nonTerminals).ToString();
                        nonTerminals.Add(newNonTerm);
                        common.Add(new RuleItem(newNonTerm, false));
                        var newRule = new Rule
                        {
                            NonTerminal = nonTerminal,
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

            return new RulesTable(rules, nonTerminals);
        }
    }
}