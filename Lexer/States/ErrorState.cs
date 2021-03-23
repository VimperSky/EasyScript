using Lexer.Types;

namespace Lexer.States
{
    public class ErrorState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (machine.IsServiceStart)
                return machine.GenerateToken(TokenType.Error).SetServiceOrComment();

            return machine.AddChar();
        }
    }
}