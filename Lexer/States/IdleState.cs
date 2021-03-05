namespace Lexer.States
{
    public class IdleState : ILexerState
    {
        public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
        {
            if (machine.IsNumberStart) return machine.SetNumberState().AddChar();

            if (machine.IsServiceStart)
                return machine.IsCommentStart ? machine.SetCommentState() : machine.SetServiceState().AddChar();

            if (machine.IsStringSymbol) return machine.SetStringState();

            if (machine.IsKeywordStart) return machine.SetKeywordState().AddChar();

            if (machine.IsIdentifierPredict) return machine.SetIdentifierState().AddChar();

            return machine.AddChar().GenerateError();
        }
    }
}