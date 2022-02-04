using System.Collections.Generic;
using Generator.Types;
using Lexer.Types;

namespace Generator.RulesProcessing
{
    public interface IRulesProcessor
    {
        TokenType ParseTokenType(string token);
        List<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules);
    }
}