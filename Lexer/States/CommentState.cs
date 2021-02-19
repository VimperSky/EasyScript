﻿using Lexer.Types;

namespace Lexer.States
{
    public class CommentState : ILexerState
    {
        private bool _commentFound;

        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (_commentFound)
                return machine.IsEndLine ? machine.GenerateToken(TokenType.Comment) : machine.AddChar();

            if (machine.IsComment)
            {
                _commentFound = true;
                return machine;
            }

            // /k
            return machine.GenerateComment().ProcessAsIdle();
        }
    }
}