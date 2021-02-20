using Lexer.Types;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine: ILexerMachine
    {
        private int _lineIndex;
        private int _charIndex;

        private bool _isFinish;
        public void PassChar(char ch)
        {
            if (ch == '\n')
            {
                _charIndex = -1;
                _lineIndex++;
            }

            ProcessChar(ch, _lineIndex, _charIndex++);
        }

        public Token GetToken()
        {
            return _tokens.Count > 0 ? _tokens.Dequeue() : null;
        }

        public void Finish()
        {
            _isFinish = true;
            PassChar(' ');
        }
    }
}