using System;
using System.Collections.Generic;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser.Actions
{
    internal static class LeftRecursionRemover
    {
        public static RuleList RemoveLeftRecursion(RuleList ruleList)
        {
            var newRules = new List<Rule>();
            var oldRules = ruleList.Rules.ToList();
            var nonTerms = ruleList.NonTerminals;

            while (oldRules.Count > 0)
            {
                var nonTerminal = oldRules[0].NonTerminal;
                var rules = ruleList.Rules.Where(x => x.NonTerminal == nonTerminal).ToList();
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
                            if (r.Items.Count == 1)
                                if (r.Items[0].Value == "e")
                                    r.Items.RemoveAt(0);
                            r.Items.Add(new RuleItem(newNonTerm, false));
                            newRules.Add(r);
                        }

                        foreach (var rest in similarRules.Select(r => r.Items.Skip(1).ToList()))
                        {
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

            return new RuleList(newRules, nonTerms);
        }
    }
}