using System.Collections.Generic;

namespace LLGenerator.Entities
{
    public class DirRule : Rule
    {
        public HashSet<string> Dirs { get; private init; }

        public static DirRule Create(HashSet<string> dirs, Rule rule)
        {
            return new() {Dirs = dirs, Items = rule.Items, NonTerminal = rule.NonTerminal};
        }

        public override string ToString()
        {
            return base.ToString() + $" [{string.Join(", ", Dirs)}]";
        }
    }
}