using Lexer.Types;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine : ILexerMachine
    {
        private int _charIndex;
        private int _lineIndex;

        public void PassChar(char ch)
        {
            ProcessChar(ch, _lineIndex, _charIndex++);
            if (ch == '\n')
            {
                _charIndex = -1;
                _lineIndex++;
            }
        }

        public Token GetToken()
        {
            return _tokens.Count > 0 ? _tokens.Dequeue() : null;
        }

        public void Finish()
        {
            IsEof = true;
            PassChar(' ');
        }
    }
}