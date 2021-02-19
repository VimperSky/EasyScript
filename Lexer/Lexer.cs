using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lexer.LexerMachine;
using Lexer.Types;

namespace Lexer
{
    public class Lexer
    {
        private readonly ILexerMachine _machine;
        private readonly StreamReader _streamReader;
        
        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new(Encoding.UTF8.GetBytes(value ?? ""));
        }
        
        public Lexer(string inputString): this(GenerateStreamFromString(inputString))
        {
        }
        
        public Lexer(Stream stream)
        {
            _streamReader = new StreamReader(stream);
            _machine = new LexerMachine.LexerMachine();
        }

        public IEnumerable<Token> Tokens
        {
            get
            {
                string line;
                while ((line = _streamReader.ReadLine()) != null)
                {
                    for (var i = 0; i <= line.Length; i++)
                    {
                        var ch = i == line.Length ? '\n' : line[i];
                        _machine.PassChar(ch);
                        Token token;
                        while ((token = _machine.GetToken()) != null)
                            yield return token;
                    }
                }
            }
        }
    }
}