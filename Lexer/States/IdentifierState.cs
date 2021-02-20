using Lexer.Types;

namespace Lexer.States
{
    public class IdentifierState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // med led
            if (machine.IsIdentifierPredict) return machine.AddChar();
            
            // led; led/ led"
            machine.GenerateToken(TokenType.Identifier).AddChar();
            if (machine.IsServiceStart) // led; led/
                return machine.SetServiceState();
            
            // led"
            return machine.GenerateError();
        }
    }
}