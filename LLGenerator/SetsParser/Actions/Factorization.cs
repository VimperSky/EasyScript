using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser.Actions
{
    internal static class Factorization
    {
        public static ImmutableList<Rule> MakeFactorization(ImmutableList<Rule> ruleList)
        {
            var newRules = new List<Rule>();
            var groups = ruleList.GetGroups();
            var nonTerms = groups.GetNonTerminals();
            foreach (var rulesGroup in groups)
            {
                var rules = rulesGroup.ToList();
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

                        var newNonTerm = SetsParserExtensions.GetNextFreeLetter(nonTerms);
                        nonTerms.Add(newNonTerm);

                        var commonFinal = rules[0].Items.Take(minCommonLen).ToList();
                        commonFinal.Add(new RuleItem(newNonTerm, false));
                        newRules.Add(new Rule
                        {
                            NonTerminal = rulesGroup.Key,
                            Items = commonFinal
                        });

                        var needE = false;
                        foreach (var index in commonIds)
                        {
                            var rest = rules[index].Items.Skip(minCommonLen).ToList();
                            if (rest.Count == 0)
                            {
                                needE = true;
                                continue;
                            }

                            newRules.Add(new Rule
                            {
                                NonTerminal = newNonTerm,
                                Items = rest
                            });
                        }

                        if (needE)
                            newRules.Add(new Rule
                            {
                                NonTerminal = newNonTerm,
                                Items = new List<RuleItem> {new(Constants.EmptySymbol, true)}
                            });

                        foreach (var index in commonIds.OrderByDescending(v => v))
                            rules.RemoveAt(index);
                    }

                newRules.AddRange(rules);
            }

            return newRules.ToImmutableList();
        }
    }
}