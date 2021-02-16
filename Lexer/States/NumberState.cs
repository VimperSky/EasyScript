namespace Lexer.States
{
    public class NumberState : ILexerState
    {
        private bool _containsPoint;

        public LexerMachine Process(LexerMachine machine)
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
            
            if (machine.IsComment) return machine.SetCommentState();


            if (machine.IsSeparator)
            {
                if (machine.IsNumberFinished) return machine.GenerateToken(TokenType.Number).GenerateServiceSymbol();

                if (machine.IsFirstService) return machine.GenerateServiceSymbol(true);
            }

            return machine.GenerateError();
        }
    }
}