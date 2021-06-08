using System.Collections.Generic;

namespace Generator.RulesParsing
{
    public interface IRulesParser
    {
        List<(string NonTerminal, string RightBody)> Parse();
    }
}