namespace Lexer.States
{
    public interface ILexerState
    {
        public LexerMachine Process(LexerMachine machine);
    }
}