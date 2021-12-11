using Lexer.Types;

namespace Lexer.LexerMachine;

public interface ILexerMachine
{
    void PassChar(char ch);
    void Finish();
    Token GetToken();
}