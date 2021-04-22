using System.Collections.Generic;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser
{
    internal class RuleList
    {
        public readonly HashSet<string> NonTerminals;
        public readonly List<Rule> Rules;

        public RuleList(List<Rule> rules, HashSet<string> nonTerminals)
        {
            Rules = rules;
            NonTerminals = nonTerminals;
        }
    }
}