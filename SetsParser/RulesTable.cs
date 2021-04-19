using System.Collections.Generic;
using Common;

namespace SetsParser
{
    internal class RulesTable
    {
        public readonly HashSet<string> NonTerminals;
        public readonly List<Rule> Rules;

        public RulesTable(List<Rule> rules, HashSet<string> nonTerminals)
        {
            Rules = rules;
            NonTerminals = nonTerminals;
        }
    }
}