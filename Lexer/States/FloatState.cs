using Lexer.Types;

namespace Lexer.States
{
    public class FloatState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // 5. .
            if (machine.IsFloatContinue)
                return machine.AddChar();

            // 5.; .5; 
            if (machine.IsServiceStart && machine.IsFloatConstructed)
            {
                machine.GenerateToken(TokenType.Float);
                return machine.SetServiceOrComment();
            }

            // 5.. .. .;
            return machine.SetError();
        }
    }
}