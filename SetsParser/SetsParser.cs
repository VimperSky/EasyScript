using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SetsParser
{
    public class SetsParser
    {
        public SetsParser(Stream input)
        {
            var baseRules = ReadRules(input);
            DoFactorization(baseRules);
            Console.WriteLine(baseRules);
        }

        private List<Rule> DoFactorization(RulesTable rulesTable)
        {
            var newRules = new List<Rule>();
            foreach (var t in rulesTable.NonTerminals)
            {
                var rules = rulesTable.Rules.Where(x => x.NonTerminal == t).ToList();
                if (rules.Count > 1)
                {
                    var common = rules.FindCommon();
                    if (common.Count > 0)
                    {
                        var newNonTerm = Extensions.GetNextFreeLetter(rulesTable.NonTerminals).ToString();
                        common.Add(new RuleItem(newNonTerm, false));
                        var newRule = new Rule
                        {
                            NonTerminal = t,
                            Items = common
                        };
                        newRules.Add(newRule);
                        newRules.Add(new Rule {NonTerminal = newNonTerm, Items = new List<RuleItem>
                        {
                            new("e", true)
                        }});
                    
                        foreach (var rule in rules)
                        {
                            var rest = rule.Items.Skip(common.Count - 1).ToList();
                            if (rest.Count == 0)
                                continue;
                            newRules.Add(new Rule {NonTerminal = newNonTerm, Items = rest});
                        }
                        continue;
                    }
                }
                newRules.AddRange(rules);
            }
            

            return newRules;
        }
        
        private RulesTable ReadRules(Stream input)
        {
            using var sr = new StreamReader(input);
            string line;
            var rawRules = new List<(string LeftBody, string RightBody)>();
            while ((line = sr.ReadLine()) != null)
            {
                var split = line.Split("->", StringSplitOptions.TrimEntries);
                var localRules = split[1].Split("|", StringSplitOptions.TrimEntries);
                rawRules.AddRange(localRules.Select(rule => (split[0], rule)));
            }
            
            var nonTerminals = rawRules.Select(x => x.LeftBody).ToImmutableHashSet();
            var rules = rawRules.Select(rawRule => new Rule
                {
                    NonTerminal = rawRule.LeftBody,
                    Items = rawRule.RightBody.Split(" ", StringSplitOptions.TrimEntries)
                        .Select(x => new RuleItem(x, !nonTerminals.Contains(x)))
                        .ToList()
                })
                .ToList();
            return new RulesTable(rules, nonTerminals);
        }
    }
}