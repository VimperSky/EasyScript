using System.Collections.Generic;

namespace SetsParser
{
    public class Rule
    {
        public string NonTerminal { get; set; }
        public List<string> Elements { get; set; }
    }
}