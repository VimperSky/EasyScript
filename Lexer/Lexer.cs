using System;
using System.Collections.Generic;
using System.IO;
using Lexer.States;

namespace Lexer
{
    public class Lexer
    {
        private readonly LexerMachine _machine;

        private readonly StreamReader _streamReader;

        public Lexer(StreamReader streamReader)
        {
            _streamReader = streamReader;
            _machine = new LexerMachine();
        }


        private void ProcessTokenValue(char ch, int lineNumber, int pos)
        {
            _machine.ProcessChar(ch, lineNumber, pos);
        }

        public IEnumerable<Token> GetTokens()
        {
            string line;
            var lineNumber = 0;
            
            void MachineOnTokenGenerated(Token obj)
            {
                yield return obj;
            }
            
            _machine.TokenGenerated += MachineOnTokenGenerated;
            while ((line = _streamReader.ReadLine()) != null) // Считываем построчно
            {
                for (var i = 0; i < line.Length; i++) ProcessTokenValue(line[i], lineNumber, i);
                ProcessTokenValue('\n', lineNumber, line.Length);

                lineNumber++;
            }
            _machine.TokenGenerated -= MachineOnTokenGenerated;

        }
    }
}