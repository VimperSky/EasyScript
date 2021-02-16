namespace Lexer.States
{
    public class IdentifierState : ILexerState
    {
        public LexerMachine Process(LexerMachine machine)
        {
            if (machine.IsStringSymbol) return machine.GenerateError();
            
            if (machine.IsComment) return machine.GenerateToken(TokenType.Identifier).SetCommentState();

            if (machine.IsSeparator) return machine.GenerateToken(TokenType.Identifier).GenerateServiceSymbol();

            if (machine.IsIdentifier) return machine.AddChar();
            
            return machine.GenerateError();
        }
    }
}