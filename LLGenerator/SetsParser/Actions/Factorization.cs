using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser.Actions
{
    internal static class Factorization
    {
        private static IEnumerable<Rule> GenerateNewRules(IList<Rule> commonRules, int commonLen, HashSet<string> nonTerms)
        {
            var newRules = new List<Rule>();
            var newNonTerm = SetsParserExtensions.GetNextFreeLetter(nonTerms);
            nonTerms.Add(newNonTerm);

            var commonFinal = commonRules[0].Items.Take(commonLen).ToList();
            commonFinal.Add(new RuleItem(newNonTerm, false));
            newRules.Add(new Rule
            {
                NonTerminal = commonRules[0].NonTerminal,
                Items = commonFinal
            });

            var needE = false;
            foreach (var t in commonRules)
            {
                var rest = t.Items.Skip(commonLen).ToList();
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
            
            return newRules;
        }

        private static List<Rule> ProcessRulesIteration(ref List<Rule> rules, HashSet<string> nonTerms)
        {
            var iterRules = new List<Rule>();
            for (var j = 0; j < rules.Count; j++)
            {
                var minCommonLen = int.MaxValue;
                var commonIds = new List<int>();
                            
                for (var i = 0; i < rules.Count; i++)
                {
                    if (rules[i].NonTerminal != rules[j].NonTerminal)
                        continue;
                    
                    var common = rules[j].FindCommon(rules[i]);
                    if (common.Count == 0)
                        continue;

                    if (common.Count < minCommonLen)
                        minCommonLen = common.Count;

                    commonIds.Add(i);
                }

                if (commonIds.Count > 1)
                {
                    var commonRules = rules.Where((_, i) => commonIds.Contains(i)).ToList();
                    rules = rules.Except(commonRules).ToList();
                    var tempRules = GenerateNewRules(commonRules, minCommonLen, nonTerms);
                    iterRules.AddRange(tempRules);
                }
            }

            return iterRules;
        }
        
        public static ImmutableList<Rule> MakeFactorization(ImmutableList<Rule> ruleList)
        {
            var newRules = new List<Rule>();
            var groups = ruleList.GetGroups();
            var nonTerms = groups.GetNonTerminals();
            foreach (var rulesGroup in groups)
            {
                var rules = rulesGroup.ToList();
                if (rules.Count > 1)
                {
                    var nextRules = new List<Rule>();
                    while (true)
                    {
                        nextRules.Clear();
                        var newNonTermRules = ProcessRulesIteration(ref rules, nonTerms);
                        nextRules.AddRange(rules);
                        nextRules.AddRange(newNonTermRules);

                        rules = nextRules.ToList();
                        if (newNonTermRules.Count == 0)
                            break;
                    }
                }
                newRules.AddRange(rules);
            }

            return newRules.ToImmutableList();
        }
    }
}