using System.Linq;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        public bool IsCommentStart => _lastChar == CommentSymbol;
        public bool IsStringSymbol => _lastChar == StringSymbol;

        public bool IsServiceStart => IsServiceSymbolStart(_lastChar);

        public bool IsFloat => IsFloatCharacter(_lastChar);

        public bool IsInt => IsDigit(_lastChar);
        
        public bool IsIdentifierPredict => IsIdentifier(_value + _lastChar);

        public bool IsKeywordStart => IsKeywordStart(_lastChar);

        public bool IsExpectedValueAchieved => _expectedValues.Contains(_value);

        public bool IsIntContinue => IsIntContinue(_value + _lastChar);
        public bool IsFloatContinue => IsFloatContinue(_value + _lastChar);
        
        public bool IsFloatConstructed => IsFloatConstructed(_value);

        public bool IsPoint => IsPoint(_lastChar);
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