using System.IO;
using System.Linq;

namespace Generator.InputParsing
{
    public class LexerInputProcessor: IInputProcessor
    {
        public string[] Parse(Stream stream)
        {
            var lexer = new Lexer.Lexer(stream);
            return lexer.Tokens.Select(x => x.ToString()).ToArray();
        }
    }
}