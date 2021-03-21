using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Types
{
    public class String
    {
        [Fact]
        public void DefaultString()
        {
            var lexer = new TestLexer("\"some string\"");

            Assert.Equal(TokenType.String, lexer.GetNextToken().Type);
        }

        [Fact]
        public void StringWithNewLine()
        {
            var lexer = new TestLexer("\"some\nstring\"");

            Assert.Equal(TokenType.String, lexer.GetNextToken().Type);
        }

        [Fact]
        public void StringWithNewLineNotClosed()
        {
            var lexer = new TestLexer("\"some\nstring");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }

        [Fact]
        public void StringWithErrorSymbol()
        {
            var lexer = new TestLexer("\"some string\"^");

            Assert.Equal(TokenType.String, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }

        [Fact]
        public void StringWithKeyword()
        {
            var lexer = new TestLexer("\"let while string\"^");

            Assert.Equal(TokenType.String, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }

        [Fact]
        public void QuotAfterString()
        {
            var lexer = new TestLexer("\"let while string\"\"");

            Assert.Equal(TokenType.String, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NotClosedString()
        {
            var lexer = new TestLexer("\"string");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }
    }
}