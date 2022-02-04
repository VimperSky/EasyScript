using System.Collections.Generic;
using Lexer.Types;

namespace Generator.InputParsing
{
    public interface IInputParser
    {
        IEnumerable<Token> Parse();
    }
}