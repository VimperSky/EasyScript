using System.Collections.Generic;
using System.Linq;
using Generator;
using Generator.Types;
using LL.Types;

namespace LL.SetsParser
{
    internal static class DirSetsFinder
    {
        internal static List<DirRule> Find(List<Rule> rules)
        {
            var foundValues = new List<HashSet<RuleItem>>();
            for (var i = 0; i < rules.Count; i++)
                foundValues.Add(new HashSet<RuleItem>());

            for (var i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (rule.Items[0].Type is ElementType.Empty)
                    foreach (var item in rules.FindNextRecursive(rule.NonTerminal))
                        foundValues[i].Add(item);
                else
                    foundValues[i].Add(rule.Items[0]);
            }

            for (;;)
            {
                var somethingChanged = false;
                foreach (var foundVal in foundValues)
                {
                    var nonTerms = foundVal.Where(x => x.Type is ElementType.NonTerminal).ToList();
                    if (nonTerms.Count > 0)
                        somethingChanged = true;
                    foreach (var nonTerm in nonTerms)
                    {
                        foundVal.Remove(nonTerm);
                        var rulesWithNonTerm = rules.Select((x, i) => (x, i))
                            .Where(x => x.x.NonTerminal == nonTerm.Value)
                            .Select(x => x.i)
                            .ToList();
                        foreach (var fVal in rulesWithNonTerm.SelectMany(rule => foundValues[rule]))
                            foundVal.Add(fVal);
                    }
                }

                if (!somethingChanged) break;
            }

            return rules.Select((t, i) => DirRule.Create(foundValues[i]
                .Select(x => x.Value).ToHashSet(), t)).ToList();
        }
    }
}