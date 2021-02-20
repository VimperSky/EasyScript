using System.Collections.Generic;
using System.IO;
using Lexer.LexerMachine;
using Lexer.Types;

namespace Lexer
{
    public class Lexer
    {
        private readonly ILexerMachine _machine;
        private readonly StreamReader _streamReader;

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
                Token token;
                while ((line = _streamReader.ReadLine()) != null)
                {
                    for (var i = 0; i <= line.Length; i++)
                    {
                        var ch = i == line.Length ? '\n' : line[i];
                        _machine.PassChar(ch);
                        while ((token = _machine.GetToken()) != null)
                            yield return token;
                    }
                }
                
                _machine.Finish();
                while ((token = _machine.GetToken()) != null)
                    yield return token;
            }
        }
        
    }
}