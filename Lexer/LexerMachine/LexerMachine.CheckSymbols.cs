using System.Linq;
using static Lexer.Constants;

namespace Lexer.LexerMachine
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

        public bool IsKeywordFinished => _expectedKeywords.Contains(_value);

        public bool IsNumberFinished => IsNumberChar(_value.Last());

        public bool IsArithmetic => _value.Length == 1 && IsSeparator(_value[0]);

        public bool IsKeywordContinue
        {
            get
            {
                {
                    var value = _value + _lastChar;
                    return _expectedKeywords.Any(x => x.Length >= value.Length && x.Substring(0, value.Length) == value);
                }
            }
        }

        public bool IsEndLine => _lastChar == EndLine;

        public bool IsEof => _isFinish;
    }
}