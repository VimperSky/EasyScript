using System.Linq;

namespace Lexer.LexerMachine;

public partial class LexerMachine
{
    public bool IsCommentSymbol => _curChar == CommentSymbol;

    public bool IsMultiCommentSymbol => _curChar == MultiCommentSymbol;

    public bool IsStringSymbol => _curChar == StringSymbol;

    public bool IsServiceStart => ServiceSymbols.Any(x => x.Key.StartsWith(_curChar));

    public bool IsFloat => IsCharFloat(_curChar);

    public bool IsInt => char.IsDigit(_curChar);

    public bool IsIdentifierPredict => IsValueIdentifier(_value + _curChar);

    public bool IsKeywordStart => KeyWords.Any(x => x.Key.StartsWith(_curChar));

    public bool IsExpectedValueAchieved => _expectedValues.Contains(_value);

    public bool IsIntContinue => _nextValue.Length is >= 1 and <= MaxIntSize && _nextValue.All(char.IsDigit);
    public bool IsFloatContinue => IsNumFloat(_nextValue);

    public bool IsFloatConstructed => IsNumFloat(_value) && _value.Any(char.IsDigit);

    public bool IsPoint => IsCharPoint(_curChar);

    public bool IsExpectedValueContinue => _expectedValues.Any(x => x.Length >= _nextValue.Length && x[.._nextValue.Length] == _nextValue);

    public bool IsEndLine => _curChar == EndLine;
    
    public bool IsEof { get; private set; }
    
    private static bool IsNumFloat(string num)
    {
        return num.Length is >= 1 and <= MaxFloatSize && num.All(IsCharFloat) && num.Count(IsCharPoint) <= 1;
    }

    private static bool IsCharFloat(char ch)
    {
        return char.IsDigit(ch) || ch == NumberPoint;
    }

    private static bool IsCharPoint(char ch)
    {
        return ch == NumberPoint;
    }
    
    private static bool IsValueIdentifier(string value) => IsCharValidStartIdentifier(value[0]) && value.Skip(1).All(IsCharValidIdentifier);

    private static bool IsCharValidIdentifier(char ch) => IsCharValidStartIdentifier(ch) || char.IsDigit(ch);

    private static bool IsCharValidStartIdentifier(char ch) => char.IsLetter(ch) || ch == Underscore;
}