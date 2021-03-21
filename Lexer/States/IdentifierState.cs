using Lexer.Types;

namespace Lexer.States
{
    public class IdentifierState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // med led
            if (machine.IsIdentifierPredict) return machine.AddChar();

            if (machine.IsServiceStart) // led; led/
                return machine.GenerateToken(TokenType.Identifier).SetServiceOrComment();

            // led"
            return machine.SetError();
        }
    }
}