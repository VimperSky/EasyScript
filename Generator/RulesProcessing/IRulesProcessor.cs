using System.Collections.Generic;
using Generator.Types;

namespace Generator.RulesProcessing
{
    public interface IRulesProcessor
    {
        string EndToken { get; }
        RuleItem ParseToken(string token);
        List<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules);
    }
}