﻿using Lexer.Types;

namespace Lexer.States
{
    public class SingleLineComment : ILexerState
    {
        private bool _isFound;
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (_isFound)
                return machine.IsEndLine ? machine.GenerateToken(TokenType.SingleComment) : machine.AddChar();
            
            // //
            if (machine.IsCommentSymbol)
            {
                _isFound = true;
                return machine.RemoveLast();
            }

            // /*
            if (machine.IsMultiCommentSymbol)
            {
                return machine.SetMultiLineCommentState();
            }

            // /a /;
            return machine.GenerateServiceSymbol().SetIdleState().ReProcess();
        }
    }
}