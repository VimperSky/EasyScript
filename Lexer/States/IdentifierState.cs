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
            machine.GenerateToken(TokenType.Identifier);
            if (machine.IsServiceStart) // led; led/
                return machine.IsCommentStart ? machine.SetCommentState() : machine.AddChar().SetServiceState();

            // led"
            return machine.AddChar().GenerateError();
        }
    }
}