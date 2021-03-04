using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Types
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
        public void NumberWithoutIntPart()
        {
            var lexer = new TestLexer(".222");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NegativeNumberWithoutIntPart()
        {
            var lexer = new TestLexer("-.22");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithoutFractionalPart()
        {
            var lexer = new TestLexer("5.");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NegativeNumberAfterNumber()
        {
            var lexer = new TestLexer("2 -5.222");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }
    }
}