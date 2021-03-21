using Lexer.Types;

namespace Lexer.States
{
    public class IntState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // 53
            if (machine.IsIntContinue)
                return machine.AddChar();

            // 53.
            if (machine.IsPoint)
                return machine.AddChar().SetFloatState(); 
            
            // 5;
            if (machine.IsServiceStart)
            {
                machine.GenerateToken(TokenType.Int);
                return machine.SetServiceOrComment();
            }

            return machine.SetError();
        }
    }
}