﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator;
using Generator.Types;

namespace LLGenerator.SetsParser
{
    internal static class LeftRecursionRemover
    {
        public static ImmutableList<Rule> RemoveLeftRecursion(ImmutableList<Rule> ruleList)
        {
            var newRules = new List<Rule>();
            var groups = ruleList.GetGroups();
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
                    var newNonTerm = LettersProvider.Instance.GetNextFreeLetter().ToString();

                    foreach (var normalRule in normalRules)
                    {
                        if (normalRule.Items[0].Type is ElementType.Empty)
                            normalRule.Items.RemoveAt(0);
                        normalRule.Items.Add(new RuleItem(newNonTerm, ElementType.NonTerminal));
                        newRules.Add(normalRule);
                    }

                    foreach (var items in recursionRules.Select(recRule => recRule.Items.Skip(1).ToList()))
                    {
                        items.Add(new RuleItem(newNonTerm, ElementType.NonTerminal));
                        newRules.Add(new Rule {NonTerminal = newNonTerm, Items = items});
                    }

                    newRules.Add(new Rule
                    {
                        NonTerminal = newNonTerm,
                        Items = new List<RuleItem> {new(Constants.EmptySymbol, ElementType.Empty)}
                    });
                }
                else
                {
                    newRules.AddRange(normalRules);
                }
            }

            return newRules.ToImmutableList();
        }
    }
}