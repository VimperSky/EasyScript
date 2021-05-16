using Lexer.Types;
using Xunit;

namespace Lexer.Tests.SeparatorsAndComments
{
    public class Semicolon
    {
        [Fact]
        public void Separator()
        {
            var lexer = new TestLexer(";");

            Assert.Equal(TokenType.Semicolon, lexer.GetNextToken().Type);
        }

        [Fact]
        public void Comma()
        {
            var lexer = new TestLexer(",");

            Assert.Equal(TokenType.Comma, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInIdentifier()
        {
            var lexer = new TestLexer("so;me");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Semicolon, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInNumber()
        {
            var lexer = new TestLexer("12,32");

            Assert.Equal(TokenType.AnyInt, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Comma, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.AnyInt, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInString()
        {
            var lexer = new TestLexer("\"so;me%^\"");

            Assert.Equal(TokenType.AnyString, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInComment()
        {
            var lexer = new TestLexer("// TODO: separators (\';\')");

            Assert.Equal(TokenType.Comment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SemicolonAfterBracket()
        {
            var lexer = new TestLexer(");");

            Assert.Equal(new Token(TokenType.CloseBracket, ")", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 0, 1).ToString(), lexer.GetNextToken().ToString());
        }

        [Fact]
        public void SemicolonAfterBrace()
        {
            var lexer = new TestLexer("};");

            Assert.Equal(new Token(TokenType.CloseBrace, "}", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 0, 1).ToString(), lexer.GetNextToken().ToString());
        }

        [Fact]
        public void SemicolonAfterBracketAndSpace()
        {
            var lexer = new TestLexer("( ;");

            Assert.Equal(new Token(TokenType.OpenBracket, "(", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 0, 2).ToString(), lexer.GetNextToken().ToString());
        }

        [Fact]
        public void SemicolonAfterBraceAndSpace()
        {
            var lexer = new TestLexer("} ;");

            Assert.Equal(new Token(TokenType.CloseBrace, "}", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 0, 2).ToString(), lexer.GetNextToken().ToString());
        }

        [Fact]
        public void DefaultString()
        {
            var lexer = new TestLexer("if; else; flex;");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Semicolon, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Semicolon, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Semicolon, lexer.GetNextToken().Type);
        }
    }
}