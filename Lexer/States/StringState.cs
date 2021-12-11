using Lexer.Types;

namespace Lexer.States;

public class StringState : ILexerState
{
    public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
    {
        // End of string
        if (machine.IsStringSymbol)
            return machine.GenerateToken(TokenType.AnyString);

        if (machine.IsEof)
            return machine.SetError();

        return machine.AddChar();
    }
}