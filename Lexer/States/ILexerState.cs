namespace Lexer.States;

public interface ILexerState
{
    public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine);
}