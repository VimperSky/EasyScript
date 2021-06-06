using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;

namespace LLGenerator.SetsParser
{
    internal static class SetsParserExtensions
    {
        public static List<IGrouping<string, Rule>> GetGroups(this List<Rule> rules)
        {
            return rules.GroupBy(x => x.NonTerminal).ToList();
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