using Lexer.Types;

namespace Lexer.States
{
    public class ServiceState: ILexerState
    {
        private readonly bool _isComment;
        public ServiceState(bool isComment)
        {
            _isComment = isComment;
        }
        
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // != //
            if (machine.IsExpectedValueContinue) return machine.AddChar();
            
            // Comment specific conditions
            if (_isComment)
            {
                // //abc //abc\n //
                if (machine.IsExpectedValueAchieved)
                    //abc\n
                    return machine.IsEndLine ? machine.GenerateToken(TokenType.Comment) : machine.AddChar();

                // /a /;
                return machine.GenerateServiceSymbol().SetServiceState().ReProcess();
            }

            // !== or !=; or !=k or !=/ or != ...
            if (machine.IsExpectedValueAchieved) return machine.GenerateServiceSymbol().SetIdleState().ReProcess();

            // !k or ! or !;
            return machine.AddChar().GenerateError();
        }
    }
}