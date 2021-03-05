using System.Collections.Generic;
using System.Linq;
using Lexer.Types;

namespace Lexer
{
    public static class Constants
    {
        public const char StringSymbol = '"';
        private const char NumberMinusSign = '-';
        private const char NumberPlusSign = '+';

        private const char NumberPoint = '.';
        public const char CommentSymbol = '/';
        public const string Comment = "//";

        public const char EndLine = '\n';
        private const char Space = ' ';
        private const char Underscore = '_';

        public static readonly List<TokenType> SkipTokens = new() {TokenType.Space, TokenType.EndLine};

        public static readonly Dictionary<string, TokenType> ServiceSymbols = new()
        {
            {Space.ToString(), TokenType.Space},
            {EndLine.ToString(), TokenType.EndLine},

            {"(", TokenType.OpenBracket},
            {")", TokenType.CloseBracket},
            {"{", TokenType.OpenBrace},
            {"}", TokenType.CloseBrace},
            {"=", TokenType.Assign},
            {";", TokenType.Separator},
            {Comment, TokenType.Comment},

            {">", TokenType.More},
            {">=", TokenType.MoreEquals},
            {"<", TokenType.Less},
            {"<=", TokenType.LessEquals},
            {"==", TokenType.Equals},
            {"!=", TokenType.NotEquals},

            {"+", TokenType.PlusOp},
            {"-", TokenType.MinusOp},
            {"*", TokenType.MultiplyOp},
            {"/", TokenType.DivOp},
            {"++", TokenType.Increment},
            {"--", TokenType.Decrement},

            {"&&", TokenType.And},
            {"||", TokenType.Or}
        };

        public static readonly string[] KeyWords =
        {
            "let", "const", "if", "else", "for", "while", "true", "false", "number", "bool", "string",
            "print", "prints", "read", "reads", "fun", "return"
        };

        public static bool IsKeywordStart(char ch)
        {
            return KeyWords.Any(x => x.StartsWith(ch));
        }

        public static bool IsServiceSymbolStart(char ch)
        {
            return ServiceSymbols.Any(x => x.Key.StartsWith(ch));
        }

        public static bool IsSign(char ch)
        {
            return ch == NumberMinusSign || ch == NumberPlusSign;
        }

        public static bool IsNumberConstructed(string num)
        {
            return IsNumberPredicted(num) && num.Any(IsDigit);
        }

        public static bool IsNumberPredicted(string num)
        {
            return num.Length >= 1 && num.All(IsNumberCharacter) && num.Count(IsPoint) <= 1 && num.Count(IsSign) <= 1;
        }

        public static bool IsNumberCharacter(char ch)
        {
            return IsDigit(ch) || IsSign(ch) || ch == NumberPoint;
        }

        private static bool IsPoint(char ch)
        {
            return ch == NumberPoint;
        }

        private static bool IsDigit(char ch)
        {
            return char.IsDigit(ch);
        }

        public static bool IsIdentifier(string text)
        {
            return IsValidIdentifierStartChar(text[0]) && text.Skip(1).All(IsValidIdentifierChar);
        }

        private static bool IsValidIdentifierChar(char ch)
        {
            return IsValidIdentifierStartChar(ch) || char.IsDigit(ch);
        }

        private static bool IsValidIdentifierStartChar(char ch)
        {
            return char.IsLetter(ch) || ch == Underscore;
        }
    }
}