namespace Lexer.States
{
    public class IdleState : ILexerState
    {
        public LexerMachine Process(LexerMachine machine)
        {
            if (machine.IsNumberStart) return machine.AddChar().SetNumberState();

            if (machine.IsComment) return machine.SetCommentState();
            
            if (machine.IsSeparator) return machine.GenerateServiceSymbol();

            if (machine.IsStringSymbol) return machine.SetStringState();

            if (machine.IsKeywordStart) return machine.SetKeyword().SetKeywordState().AddChar();

            if (machine.IsIdentifier) return machine.SetIdentifierState().AddChar();

            return machine.GenerateError();
        }
    }
}