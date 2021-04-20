using System.Collections.Generic;

namespace Common
{
    public class TableRule
    {
        public int Id { get; init; }
        public string NonTerminal { get; init; }
        public HashSet<string> DirSet { get; init; }
        public int? GoTo { get; set; }
        public bool IsError { get; init; }

        public override string ToString()
        {
            var nonTerminalToString = NonTerminal;
            if (nonTerminalToString == "\n") nonTerminalToString = "\\n";
            return $"{Id}   {nonTerminalToString}   {string.Join(", ", DirSet)} {GoTo} {IsError}";
        }
    }
}