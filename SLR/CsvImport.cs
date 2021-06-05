using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public class CsvImport
    {
        public static ImmutableList<Rule> Parse(Stream stream)
        {
            using var sr = new StreamReader(stream);
            string line;
            var rawRules = new List<(string LeftBody, string RightBody)>();
            while ((line = sr.ReadLine()) != null)
            {
                var split = line.Split("->", StringSplitOptions.TrimEntries);
                var localRules = split[1].Split("|", StringSplitOptions.TrimEntries);
                rawRules.AddRange(localRules.Select(rule => (split[0], rule)));
            }

            var nonTerminals = rawRules.Select(x => x.LeftBody).ToHashSet();

            var rules = rawRules.Select(rawRule => new Rule
                {
                    NonTerminal = rawRule.LeftBody,
                    Items = rawRule.RightBody.Split(" ", StringSplitOptions.TrimEntries)
                        .Select(x => nonTerminals.Contains(x)
                            ? new RuleItem(x, ElementType.NonTerminal)
                            : x == Constants.EmptySymbol
                                ? new RuleItem(x, ElementType.Empty)
                                : x == Constants.EndSymbol
                                    ? new RuleItem(x, ElementType.End)
                                    : new RuleItem(x, ElementType.Terminal))
                        .ToList()
                })
                .ToList();
            

            return rules.ToImmutableList();
        }
    }
}