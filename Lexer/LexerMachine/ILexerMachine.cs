using Lexer.Types;

namespace Lexer.LexerMachine
{
    public interface ILexerMachine
    { 
        public void PassChar(char ch);
        Token GetToken();
    }
}