using Lexer.Types;

namespace Lexer.States
{
    public class NumberState : ILexerState
    {
        private bool _containsPoint;

        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            // -5 or 53
            if (machine.IsDigit)
                return machine.AddChar();

            if (machine.IsPoint)
            {
                if (_containsPoint)
                    return machine.GenerateError();

                _containsPoint = true;
                return machine.AddChar();
            }
            
            if (machine.IsComment)
            {
                if (machine.IsNumberFinished) return machine.GenerateToken(TokenType.Number).SetCommentState();
                
                // +//
                if (machine.IsArithmetic) return machine.GenerateServiceSymbol(true);
            }
            
            if (machine.IsSeparator)
            {
                if (machine.IsNumberFinished) return machine.GenerateToken(TokenType.Number).GenerateServiceSymbol();

                // -;
                if (machine.IsArithmetic) return machine.GenerateServiceSymbol(true);
            }

            return machine.GenerateError();
        }
    }
}