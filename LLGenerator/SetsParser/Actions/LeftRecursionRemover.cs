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
            var nonTerms = ruleList.NonTerminals;
            var groups = ruleList.Rules.GroupBy(x => x.NonTerminal);
            foreach (var rules in groups)
            {
                var recursionRules = new List<Rule>();
                var normalRules = new List<Rule>();
                foreach (var rule in rules)
                    if (rule.Items[0].Value == rule.NonTerminal)
                        recursionRules.Add(rule);
                    else
                        normalRules.Add(rule);

                if (recursionRules.Count > 0)
                {
                    var newNonTerm = SetsParserExtensions.GetNextFreeLetter(nonTerms).ToString();
                    nonTerms.Add(newNonTerm);

                    foreach (var normalRule in normalRules)
                    {
                        if (normalRule.Items[0].Value == "e")
                            normalRule.Items.RemoveAt(0);
                        normalRule.Items.Add(new RuleItem(newNonTerm, false));
                        newRules.Add(normalRule);
                    }
                    
                    foreach (var recRule in recursionRules)
                    {
                        var items = recRule.Items.Skip(1).ToList();
                        items.Add(new RuleItem(newNonTerm, false));
                        newRules.Add(new Rule { NonTerminal = newNonTerm, Items = items});
                    }
                    
                    newRules.Add(new Rule {NonTerminal = newNonTerm, Items = new List<RuleItem>
                    {
                        new("e", true)
                    }});
                }
                else
                {
                    newRules.AddRange(normalRules);
                }

            }

            return new RuleList(newRules, nonTerms);
        }
    }
}