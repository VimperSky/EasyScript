using System.Linq;
using static Lexer.Constants;

namespace Lexer
{
    public partial class LexerMachine
    {
        public bool IsComment => _lastChar == CommentSymbol;
        public bool IsStringSymbol => _lastChar == StringSymbol;

        public bool IsDigit => IsDigit(_lastChar);

        public bool IsPoint => _lastChar == NumberPoint;

        public bool IsSeparator => IsSeparator(_lastChar);

        public bool IsNumberStart => IsNumberStart(_lastChar);

        public bool IsIdentifier => IsIdentifier(_value + _lastChar);

        public bool IsKeywordStart => IsKeywordStart(_lastChar);

        public bool IsKeywordFinished => _value == _expectedValue;

        public bool IsNumberFinished => IsDigit(_value.Last());

        public bool IsFirstService => IsSeparator(_value[0]);

        public bool IsKeywordContinue
        {
            get
            {
                {
                    var value = _value + _lastChar;
                    return _expectedValue.Length >= value.Length
                           && value == _expectedValue.Substring(0, value.Length);
                }
            }
        }

        public bool IsEndLine => _lastChar == EndLine;
    }
}