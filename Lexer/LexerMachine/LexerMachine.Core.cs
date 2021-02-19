using System.Collections.Generic;
using Lexer.Types;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        private readonly Queue<Token> _tokens = new();

        private char _lastChar;
        
        private int _lastLine;
        private int _lastPos;

        private int _startLine;
        private int _startPos;

        private string _expectedValue;
        private string _value;

        public LexerMachine()
        {
            Reset();
        }
        
        private void ProcessChar(char ch, int line, int pos)
        {
            if (_startPos == -1)
                _startPos = pos;
            if (_startLine == -1)
                _startLine = line;

            _lastLine = line;
            _lastChar = ch;
            _lastPos = pos;

            _lexerState.Process(this);
        }

        public LexerMachine ProcessAsIdle()
        {
            SetIdleState();
            return _lexerState.Process(this);
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
            if (SkipTokens.Contains(tokenType))
                return Reset();
            var newToken = _startPos == -1 ? new Token(tokenType, _value, _lastLine, _lastPos) : 
                new Token(tokenType, _value, _startLine, _startPos);
            _tokens.Enqueue(newToken);
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

        public LexerMachine GenerateComment()
        {
            return AddChar(CommentSymbol).GenerateToken(ServiceSymbols[CommentSymbol.ToString()]);
        }
    }
}