using Lexer.Types;
using Xunit;

namespace Lexer.Tests
{
    public class Number
    {
        [Fact]
        public void DefaultNumber()
        {
            var lexer = new TestLexer("5");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithMultipleCharacters()
        {
            var lexer = new TestLexer("52");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithFloat()
        {
            var lexer = new TestLexer("5.222");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NegativeNumber()
        {
            var lexer = new TestLexer("-5.222");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NegativeNumberAfterNumber()
        {
            var lexer = new TestLexer("2 -5.222");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }
    }
}