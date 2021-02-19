﻿using Lexer.Types;

namespace Lexer.States
{
    public class StringState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // End of string
            if (machine.IsStringSymbol)
                return machine.GenerateToken(TokenType.String);

            return machine.AddChar();
        }
    }
}