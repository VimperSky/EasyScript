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
                if (machine.IsNumberConstructed)
                {
                    machine.GenerateToken(TokenType.Number);
                    return machine.IsCommentStart ? machine.SetCommentState() : machine.SetServiceState().AddChar();
                }

                // +; +.;
                if (machine.IsNumberStartsFromSign)
                {
                    machine.GenerateServiceSymbol(true);

                    return machine.IsCommentStart ? machine.SetCommentState() : machine.SetServiceState().AddChar();
                }
            }

            return machine.GenerateError();
        }
    }
}