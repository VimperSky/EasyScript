using Lexer.Types;

namespace Lexer.States
{
    public class KeywordState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // let
            if (machine.IsExpectedValueContinue) return machine.AddChar();

            // letk lek
            if (machine.IsIdentifierPredict) return machine.SetIdentifierState().AddChar();

            // let; let" let/
            if (machine.IsExpectedValueAchieved)
            {
                machine.GenerateToken(TokenType.KeyWord);

                if (machine.IsServiceStart) return machine.IsCommentStart ? machine.SetCommentState() :  machine.SetServiceState().AddChar();
                
                if (machine.IsStringSymbol) return machine.GenerateError();
            }

            // le; le" le/
            return machine.SetIdentifierState().ReProcess();
        }
    }
}