using System;
using System.Collections.Generic;
using System.Linq;
using Lexer.Types;

namespace Lexer.LexerMachine;

public partial class LexerMachine
{
    private const char StringSymbol = '"';
    private const char CommentSymbol = '/';
    private const char MultiCommentSymbol = '*';
    private const char EndLine = '\n';
    private const string SingleComment = "//";
    private const char NumberPoint = '.';
    private const string MultiCommentStart = "/*";
    private const char Space = ' ';
    private const char Underscore = '_';
    private const byte MaxIntSize = 19; // long = 19 characters
    private const byte MaxFloatSize = 38;

    private static readonly List<TokenType> SkipTokens = new() { TokenType.Space, TokenType.NewLine };

    public static readonly Dictionary<string, TokenType> ServiceSymbols = new()
    {
        { Space.ToString(), TokenType.Space },
        { EndLine.ToString(), TokenType.NewLine },

        { "(", TokenType.OpenParenthesis },
        { ")", TokenType.CloseParenthesis },
        
        { "[", TokenType.OpenSquareBracket },
        { "]", TokenType.CloseSquareBracket },
        
        { "{", TokenType.OpenBrace },
        { "}", TokenType.CloseBrace },
        
        { "=", TokenType.Assign },
        { ";", TokenType.Semicolon },
        { ",", TokenType.Comma },
        
        { SingleComment, TokenType.Comment },
        { MultiCommentStart, TokenType.MultiComment },

        { ">", TokenType.More },
        { ">=", TokenType.MoreEquals },
        { "<", TokenType.Less },
        { "<=", TokenType.LessEquals },
        { "==", TokenType.Equals },
        { "!=", TokenType.NotEquals },

        { "+", TokenType.PlusOp },
        { "-", TokenType.MinusOp },
        { "*", TokenType.MultiplyOp },
        { "/", TokenType.DivOp },
        { "++", TokenType.Increment },
        { "--", TokenType.Decrement },

        { "&", TokenType.And },
        { "|", TokenType.Or }
    };
    
    public static readonly Dictionary<string, TokenType> KeyWords = Enum.GetValues<TokenType>()
        .Where(x => (byte)x >= 100)
        .ToDictionary(x => x.ToString().ToLower1());
}