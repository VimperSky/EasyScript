using System;
using System.Collections.Generic;
using System.IO;
using Lexer.States;

namespace Lexer
{
    public class Lexer
    {
        private readonly LexerMachine _machine;
        private readonly List<Token> _tokens;

        public Lexer()
        {
            _tokens = new List<Token>();
            _machine = new LexerMachine();
        }

        private void ProcessTokenValue(char ch, int lineNumber, int pos)
        {
            _machine.ProcessChar(ch, lineNumber, pos);
        }

        public void Run(StreamReader sr, StreamWriter sw)
        {
            string line;
            var lineNumber = 0;
            while ((line = sr.ReadLine()) != null) // Считываем построчно
            {
                for (var i = 0; i < line.Length; i++) ProcessTokenValue(line[i], lineNumber, i);
                // После конца строки прогоняем еще один пробел в лексер как разделитель строки. Возможно потом нужно сделать другой тип разделителя.
                ProcessTokenValue('\n', lineNumber, line.Length);
                _tokens.AddRange(_machine.GetTokens());

                lineNumber++;
            }

            foreach (var token in _tokens)
                Console.WriteLine(token);
        }
    }
}