using System.Collections.Immutable;
using LLGenerator.Types;

namespace LLGenerator
{
    public class RulesKeeper
    {
        public ImmutableList<DirRule> DirRules { get; set; }
    }
}