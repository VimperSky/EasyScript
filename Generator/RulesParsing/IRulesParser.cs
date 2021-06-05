using System.Collections.Generic;
using System.IO;

namespace Generator.RulesParsing
{
    public interface IRulesParser
    {
        List<(string NonTerminal, string RightBody)> Parse();
    }
}