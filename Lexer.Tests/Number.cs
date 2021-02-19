using System.Linq;
using Xunit;
using Lexer.States;
using Lexer.Types;

namespace Lexer.Tests
{
    public class Number
    {
        
        [Fact]
        public void DefaultNumber()
        {
            var lexer = new Lexer("5");

            Assert.Equal(TokenType.Number, lexer.Tokens.First().Type);
        }
        
        [Fact]
        public void NumberWithMultipleCharacters()
        {
            var lexer = new Lexer("52");
            
            Assert.Equal(TokenType.Number, lexer.Tokens.First().Type);
        }
        
        [Fact]
        public void NumberWithFloat()
        {
            var lexer = new Lexer("5.222");

            Assert.Equal(TokenType.Number, lexer.Tokens.First().Type);
        }
        
        [Fact]
        public void NegativeNumber()
        {
            var lexer = new Lexer("-5.222");

            Assert.Equal(TokenType.Number, lexer.Tokens.First().Type);
        }
        
        [Fact]
        public void NegativeNumberAfterNumber()
        {
            var lexer = new Lexer("2 -5.222");

            Assert.Equal(TokenType.Number, lexer.Tokens.First().Type);
        }
    }
}