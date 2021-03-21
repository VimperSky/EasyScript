using Lexer.Types;

namespace Lexer.States
{
    public class MultiComment: ILexerState
    {
        private bool _isProbablyEnd;
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (machine.IsEof)
                return machine.GenerateToken(TokenType.MultiComment);

            if (_isProbablyEnd)
            {
                _isProbablyEnd = false;

                if (machine.IsCommentSymbol)
                    return machine.RemoveLast().GenerateToken(TokenType.MultiComment);
            }
            
            if (machine.IsMultiCommentSymbol)
            {
                _isProbablyEnd = true;
            }

            return machine.AddChar();
        }
    }
}