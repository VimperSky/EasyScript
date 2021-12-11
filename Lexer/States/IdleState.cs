namespace Lexer.States;

public class IdleState : ILexerState
{
    public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
    {
        if (machine.IsInt) return machine.SetIntState().AddChar();

        if (machine.IsFloat) return machine.SetFloatState().AddChar();

        if (machine.IsServiceStart)
            return machine.SetServiceOrComment();

        if (machine.IsStringSymbol) return machine.SetStringState();

        if (machine.IsKeywordStart) return machine.SetKeywordState().AddChar();

        if (machine.IsIdentifierPredict) return machine.SetIdentifierState().AddChar();

        return machine.SetError();
    }
}