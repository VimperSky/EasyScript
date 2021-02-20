using System.Linq;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        private bool IsCommentStart => _lastChar == CommentSymbol;
        public bool IsStringSymbol => _lastChar == StringSymbol;
        
        public bool IsServiceStart => IsServiceSymbolStart(_lastChar);
        
        public bool IsNumberStart => IsNumberCharacter(_lastChar);

        public bool IsIdentifierPredict => IsIdentifier(_value + _lastChar);

        public bool IsKeywordStart => IsKeywordStart(_lastChar);

        public bool IsExpectedValueAchieved => _expectedValues.Contains(_value);

        public bool IsNumberPredict => IsNumberPredicted(_value + _lastChar);
        public bool IsNumberConstructed => IsNumberConstructed(_value);

        public bool IsNumberStartsFromSign => _value.Length >= 1 && IsSign(_value[0]);

        public bool IsExpectedValueContinue
        {
            get
            {
                {
                    var value = _value + _lastChar;
                    return _expectedValues.Any(x => x.Length >= value.Length && x.Substring(0, value.Length) == value);
                }
            }
        }

        public bool IsEndLine => _lastChar == EndLine;

        public bool IsEof { get; private set; }
    }
}