using System.Collections.Generic;
using System.Collections.Immutable;
using Generator.Types;

namespace Generator
{
    public static class Extensions
    {
        public static IEnumerable<RuleItem> FindNextRecursive(this ImmutableList<Rule> rules, string nonTerm)
        {
            return FindUp(rules, nonTerm, new HashSet<int>());
        }

        private static IEnumerable<RuleItem> FindUp(ImmutableList<Rule> rules, string nonTerm, ISet<int> history)
        {
            var returns = new HashSet<RuleItem>();
            for (var i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                for (var j = 0; j < rule.Items.Count; j++)
                    if (rule.Items[j].Value == nonTerm)
                    {
                        if (++j < rule.Items.Count)
                        {
                            returns.Add(rule.Items[j]);
                        }
                        else
                        {
                            if (history.Contains(i)) return returns;
                            history.Add(i);
                            var nextReturns = FindUp(rules, rule.NonTerminal, history);
                            foreach (var item in nextReturns)
                                returns.Add(item);
                        }
                    }
            }

            return returns;
        }
    }
}