using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lexer
{
    public static class Constants
    {
        public const char StringSymbol = '"';
        private const char NumberMinusSign = '-';
        private const char NumberPlusSign = '+';

        public const char NumberPoint = '.';
        public const char CommentSymbol = '/';
        
        public const char EndLine = '\n';
        private const char Space = ' ';

        public static readonly List<TokenType> SkipSymbols = new() {TokenType.Space, TokenType.EndLine};
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
            {"//", TokenType.Comment},

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

            {"&", TokenType.And},
            {"|", TokenType.Or}
        };

        private static readonly string[] KeyWords = {"let", "if", "else", "while", "true", "false"};
        public static readonly Dictionary<char, string> KeyWordsCheck;

        static Constants()
        {
            KeyWordsCheck = KeyWords.ToDictionary(x => x[0], x => x);
        }

        public static bool IsKeywordStart(char ch)
        {
            return KeyWordsCheck.ContainsKey(ch);
        }

        public static bool IsSeparator(char ch)
        {
            return ServiceSymbols.ContainsKey(ch.ToString());
        }

        /*public static bool IsNumberConstructed(string text)
        {
            return text.Length > text.IndexOf(NumberPoint) + 1 && (IsSign(text[0]) && text.Length > 1 || IsDigit(text[0]));
        }*/

        private static bool IsSign(char ch)
        {
            return ch == NumberMinusSign || ch == NumberPlusSign;
        }

        public static bool IsNumberStart(char ch)
        {
            return IsDigit(ch) || IsSign(ch);
        }

        public static bool IsDigit(char ch)
        {
            return char.IsDigit(ch);
        }

        public static bool IsIdentifier(string text)
        {
            return IsIdentifierStart(text[0]) && text.Skip(1).All(x => char.IsLetterOrDigit(x) || x == '_');
        }

        private static bool IsIdentifierStart(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }
    }
}