using Lexer.Types;

namespace Lexer.States
{
    public class SingleComment : ILexerState
    {
        private bool _isFound;
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (_isFound)
                return machine.IsEndLine ? machine.GenerateToken(TokenType.Comment) : machine.AddChar();
            
            // //
            if (machine.IsCommentSymbol)
            {
                _isFound = true;
                return machine.RemoveChar();
            }

            // /*
            if (machine.IsMultiCommentSymbol)
            {
                return machine.RemoveChar().SetMultiLineCommentState();
            }

            // /a /;
            return machine.GenerateServiceSymbol().SetIdleState().ReProcess();
        }
    }
}