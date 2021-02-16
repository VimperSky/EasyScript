using System.Collections.Generic;
using System.Linq;
using static Lexer.Constants;

namespace Lexer.States
{
    public partial class LexerMachine
    {
        private readonly List<Token> _tempTokens = new();
        private string _expectedValue;
        private char _lastChar;
        private int _lastLine;
        private int _lastPos;

        private int _startLine;
        private int _startPos;

        private string _value;

        public LexerMachine()
        {
            Reset();
        }

        public LexerMachine ProcessChar(char ch, int line, int pos)
        {
            if (_startPos == -1)
                _startPos = pos;
            if (_startLine == -1)
                _startLine = line;

            _lastLine = line;
            _lastChar = ch;
            _lastPos = pos;

            return _lexerState.Process(this);
        }

        public LexerMachine ProcessAsIdle()
        {
            return Reset().ProcessChar(_lastChar, _lastLine, _lastPos);
        }

        public IEnumerable<Token> GetTokens()
        {
            var retTokens = _tempTokens.Where(x => !SkipSymbols.Contains(x.Type)).ToArray();
            _tempTokens.Clear();
            return retTokens;
        }

        public LexerMachine AddChar()
        {
            return AddChar(_lastChar);
        }

        private LexerMachine AddChar(char ch)
        {
            _value += ch;
            return this;
        }

        public LexerMachine SetKeyword()
        {
            _expectedValue = _lastChar.ToString();
            return this;
        }

        private LexerMachine Reset()
        {
            _value = "";
            _expectedValue = "";
            _startPos = -1;
            _startLine = -1;

            return SetIdleState();
        }

        public LexerMachine GenerateToken(TokenType tokenType)
        {
            _tempTokens.Add(new Token(tokenType, _value, _startLine, _startPos));
            return Reset();
        }

        public LexerMachine GenerateError()
        {
            return AddChar().GenerateToken(TokenType.Error);
        }

        public LexerMachine GenerateServiceSymbol(bool first = false)
        {
            return first
                ? GenerateToken(ServiceSymbols[_value[0].ToString()])
                : AddChar().GenerateToken(ServiceSymbols[_lastChar.ToString()]);
        }

        public LexerMachine GenerateComment() => AddChar(CommentSymbol).GenerateToken(ServiceSymbols[CommentSymbol.ToString()]);
    }
}