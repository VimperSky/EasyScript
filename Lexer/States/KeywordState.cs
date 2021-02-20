using Lexer.Types;

namespace Lexer.States
{
    public class KeywordState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (machine.IsStringSymbol) return machine.GenerateError();

            if (machine.IsComment) return machine.SetCommentState();

            // le; or let; or let 
            if (machine.IsSeparator) return machine.GenerateToken(machine.IsKeywordFinished ? TokenType.KeyWord : TokenType.Identifier).GenerateServiceSymbol();

            // l"e"
            if (machine.IsKeywordContinue) return machine.AddChar();

            // led or letd
            if (machine.IsIdentifier) return machine.AddChar().SetIdentifierState();

            return machine.GenerateError();
        }
    }
}