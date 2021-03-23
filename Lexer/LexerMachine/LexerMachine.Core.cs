using System;
using System.Collections.Generic;
using Lexer.Types;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        private readonly Queue<Token> _tokens = new();

        private string[] _expectedValues;

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

        public LexerMachine ReProcess()
        {
            return _lexerState.Process(this);
        }

        public LexerMachine AddChar()
        {
            return AddChar(_lastChar);
        }

        public LexerMachine RemoveChar()
        {
            _value = _value.Substring(0, _value.Length - 1);
            return this;
        }

        private LexerMachine AddChar(char ch)
        {
            _value += ch;
            return this;
        }

        private LexerMachine Reset()
        {
            _value = "";
            _expectedValues = Array.Empty<string>();
            _startPos = -1;
            _startLine = -1;

            return SetIdleState();
        }

        public LexerMachine GenerateToken(TokenType tokenType)
        {
            if (SkipTokens.Contains(tokenType))
                return Reset();
            var newToken = _startPos == -1
                ? new Token(tokenType, _value, _lastLine, _lastPos)
                : new Token(tokenType, _value, _startLine, _startPos);
            _tokens.Enqueue(newToken);
            return Reset();
        }

        public LexerMachine GenerateServiceSymbol()
        {
            return GenerateToken(ServiceSymbols[_value]);
        }
    }
}