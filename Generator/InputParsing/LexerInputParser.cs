using System.Collections.Generic;
using System.IO;
using Lexer.Types;

namespace Generator.InputParsing
{
    public class LexerInputParser : IInputParser
    {
        private readonly string _path;

        public LexerInputParser(string path)
        {
            _path = path;
        }


        public IEnumerable<Token> Parse()
        {
            var lexer = new Lexer.Lexer(File.OpenRead(_path));
            return lexer.Tokens;
        }
    }
}