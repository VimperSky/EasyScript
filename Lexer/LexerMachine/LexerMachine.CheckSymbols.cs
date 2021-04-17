using System.Linq;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        public bool IsCommentSymbol => _curChar == CommentSymbol;

        public bool IsMultiCommentSymbol => _curChar == MultiCommentSymbol;

        public bool IsStringSymbol => _curChar == StringSymbol;

        public bool IsServiceStart => IsServiceSymbolStart(_curChar);

        public bool IsFloat => IsFloatCharacter(_curChar);

        public bool IsInt => IsDigit(_curChar);

        public bool IsIdentifierPredict => IsIdentifier(_value + _curChar);

        public bool IsKeywordStart => IsKeywordStart(_curChar);

        public bool IsExpectedValueAchieved => _expectedValues.Contains(_value);

        public bool IsIntContinue => IsIntContinue(_value + _curChar);
        public bool IsFloatContinue => IsFloatContinue(_value + _curChar);

        public bool IsFloatConstructed => IsFloatConstructed(_value);

        public bool IsPoint => IsPoint(_curChar);

        public bool IsExpectedValueContinue
        {
            get
            {
                {
                    var value = _value + _curChar;
                    return _expectedValues.Any(x => x.Length >= value.Length && x.Substring(0, value.Length) == value);
                }
            }
        }

        public bool IsEndLine => _curChar == EndLine;

        public bool IsEof { get; private set; }
    }
}