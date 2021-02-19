using System.Collections.Generic;
using System.IO;
using System.Text;
using Lexer.Types;

namespace Lexer.Tests
{
    public class TestLexer : Lexer
    {
        private readonly IEnumerator<Token> _tokenEnumerator;

        public TestLexer(string inputString) : base(GenerateStreamFromString(inputString))
        {
            _tokenEnumerator = Tokens.GetEnumerator();
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new(Encoding.UTF8.GetBytes(value ?? ""));
        }

        public Token GetNextToken()
        {
            return _tokenEnumerator.MoveNext() ? _tokenEnumerator.Current : null;
        }
    }
}