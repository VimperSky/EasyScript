using System;
using System.Collections.Generic;
using Lexer.States;
using Lexer.Types;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        private readonly Queue<Token> _tokens = new();

        private string[] _expectedValues;

        private char _curChar;

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
            if (_lexerState is IdleState)
            {
                _startPos = pos;
                _startLine = line;
            }
            _lastLine = line;
            _lastPos = pos;
            _curChar = ch;
            _lexerState.Process(this);
        }

        public LexerMachine ReProcess()
        {
            return _lexerState.Process(this);
        }

        public LexerMachine AddChar()
        {
            return AddChar(_curChar);
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
            _startPos = _lastPos;
            _startLine = _lastLine;

            return SetIdleState();
        }

        public LexerMachine GenerateToken(TokenType tokenType)
        {
            if (SkipTokens.Contains(tokenType))
                return Reset();
            var newToken = new Token(tokenType, _value, _startLine, _startPos);
            _tokens.Enqueue(newToken);
            return Reset();
        }

        public LexerMachine GenerateServiceSymbol()
        {
            return GenerateToken(ServiceSymbols[_value]);
        }
    }
}