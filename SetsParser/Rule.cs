using System.Collections.Generic;
using System.Linq;

namespace SetsParser
{
    public class Rule
    {
        public string NonTerminal { get; init; }
        public List<RuleItem> Items { get; init; }
        
        public override string ToString()
        {
            return $"{NonTerminal} -> {string.Join(" ", Items.Select(x => x.Value))}";
        }
    }
}