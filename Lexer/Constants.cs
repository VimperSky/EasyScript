using System;
using System.Collections.Generic;
using System.Linq;
using Lexer.Types;

namespace Lexer
{
    public static class Constants
    {
        public const char StringSymbol = '"';

        private const char NumberPoint = '.';
        public const char CommentSymbol = '/';
        public const char MultiCommentSymbol = '*';

        private const string MultiCommentStart = "/*";
        public const string SingleComment = "//";

        public const char EndLine = '\n';
        private const char Space = ' ';
        private const char Underscore = '_';

        private const byte MaxIntSize = 19; // long = 19 characters
        private const byte MaxFloatSize = 38;

        public static readonly List<TokenType> SkipTokens = new() {TokenType.Space, TokenType.NewLine};

        public static readonly Dictionary<string, TokenType> ServiceSymbols = new()
        {
            {Space.ToString(), TokenType.Space},
            {EndLine.ToString(), TokenType.NewLine},

            {"(", TokenType.OpenBracket},
            {")", TokenType.CloseBracket},
            {"{", TokenType.OpenBrace},
            {"}", TokenType.CloseBrace},
            {"=", TokenType.Assign},
            {";", TokenType.Semicolon},
            {",", TokenType.Comma},
            {SingleComment, TokenType.Comment},
            {MultiCommentStart, TokenType.MultiComment},

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

            {"&", TokenType.And},
            {"|", TokenType.Or},
        };

        
        public static readonly Dictionary<string, TokenType> KeyWords = Enum.GetValues<TokenType>()
            .Where(x => (byte) x >= 100)
            .ToDictionary(x => x.ToString().ToLower1());
        

        public static bool IsKeywordStart(char ch)
        {
            return KeyWords.Any(x => x.Key.StartsWith(ch));
        }

        public static bool IsServiceSymbolStart(char ch)
        {
            return ServiceSymbols.Any(x => x.Key.StartsWith(ch));
        }

        public static bool IsFloatConstructed(string num)
        {
            return IsFloatContinue(num) && num.Any(IsDigit);
        }

        public static bool IsIntContinue(string num)
        {
            return num.Length >= 1 && num.Length <= MaxIntSize && num.All(IsDigit);
        }

        public static bool IsFloatContinue(string num)
        {
            return num.Length >= 1 && num.Length <= MaxFloatSize && num.All(IsFloatCharacter) &&
                   num.Count(IsPoint) <= 1;
        }

        public static bool IsFloatCharacter(char ch)
        {
            return IsDigit(ch) || ch == NumberPoint;
        }

        public static bool IsPoint(char ch)
        {
            return ch == NumberPoint;
        }

        public static bool IsDigit(char ch)
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