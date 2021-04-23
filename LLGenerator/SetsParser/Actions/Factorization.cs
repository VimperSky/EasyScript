using System.Collections.Generic;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser.Actions
{
    internal static class Factorization
    {
        public static RuleList MakeFactorization(RuleList ruleList)
        {
            var newRules = new List<Rule>();
            var oldRules = ruleList.Rules.ToList();

            var nonTerminals = ruleList.NonTerminals;
            while (oldRules.Count > 0)
            {
                var nonTerminal = oldRules[0].NonTerminal;
                var rules = ruleList.Rules.Where(x => x.NonTerminal == nonTerminal).ToList();
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

                        // Если не найдено общих элементов то выходим отсюда от греха подальше!!
                        if (commonIds.Count == 1)
                            break;

                        var newNonTerm = SetsParserExtensions.GetNextFreeLetter(nonTerminals).ToString();
                        nonTerminals.Add(newNonTerm);

                        var commonFinal = rules[0].Items.Take(minCommonLen).ToList();
                        commonFinal.Add(new RuleItem(newNonTerm, false));
                        newRules.Add(new Rule
                        {
                            NonTerminal = nonTerminal,
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
                                Items = new List<RuleItem> {new("e", true)}
                            });

                        foreach (var index in commonIds.OrderByDescending(v => v))
                            rules.RemoveAt(index);
                    }

                newRules.AddRange(rules);
            }

            return new RuleList(newRules, nonTerminals);
        }
    }
}