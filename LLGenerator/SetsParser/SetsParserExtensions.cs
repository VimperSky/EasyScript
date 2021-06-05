using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;
using LLGenerator.Types;

namespace LLGenerator.SetsParser
{
    internal static class SetsParserExtensions
    {
        public static ImmutableList<IGrouping<string, Rule>> GetGroups(this ImmutableList<Rule> rules)
        {
            return rules.GroupBy(x => x.NonTerminal).ToImmutableList();
        }

        public static HashSet<string> GetNonTerminals(this ImmutableList<IGrouping<string, Rule>> groups)
        {
            return groups.Select(x => x.Key).ToHashSet();
        }
        
        public static List<RuleItem> FindCommon(this Rule a, Rule b)
        {
            var minLen = Math.Min(a.Items.Count, b.Items.Count);
            var common = new List<RuleItem>();
            for (var i = 0; i < minLen; i++)
            {
                if (!a.Items[i].Equals(b.Items[i]))
                    break;
                common.Add(a.Items[i]);
            }

            return common;
        }
    }
}