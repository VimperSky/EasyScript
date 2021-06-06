using System.Collections.Generic;
using Generator.Types;

namespace Generator.RulesProcessing
{
    public interface IRulesProcessor
    {
        RuleItem ParseToken(string token);
        List<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules);
    }
}