using System.Collections.Generic;
using System.Collections.Immutable;
using Generator.Types;

namespace Generator.RulesProcessing
{
    public interface IRulesProcessor
    {
        RuleItem ParseToken(string token);
        ImmutableList<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules);
    }
}