using System.Collections.Generic;
using System.Collections.Immutable;

namespace SetsParser
{
    public class RulesTable
    {
        public readonly List<Rule> Rules;
        public readonly ImmutableHashSet<string> NonTerminals;

        public RulesTable(List<Rule> rules, ImmutableHashSet<string> nonTerminals)
        {
            Rules = rules;
            NonTerminals = nonTerminals;
        }
    }
}