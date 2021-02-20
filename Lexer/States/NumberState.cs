using Lexer.Types;

namespace Lexer.States
{
    public class NumberState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // +5 53
            if (machine.IsNumberPredict)
                return machine.AddChar();

            // +; +5; 5; .;
            if (machine.IsServiceStart)
            {
                // +5; .5; 5;  
                if (machine.IsNumberConstructed) return machine.GenerateToken(TokenType.Number).SetServiceState().AddChar();

                // +; +.;
                if (machine.IsNumberStartsFromSign) return machine.GenerateServiceSymbol(true).SetServiceState().AddChar();
            }
            
            return machine.GenerateError();
        }
    }
}