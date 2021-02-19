using System.Linq;
using Xunit;
using Lexer.States;
using Lexer.Types;

namespace Lexer.Tests
{
    public class String
    {
        [Fact]
        public void DefaultString()
        {
            var lexer = new Lexer("\"some string\"");

            Assert.Equal(TokenType.String, lexer.Tokens.First().Type);
        }
        
        [Fact]
        public void StringWithNewLine()
        {
            var lexer = new Lexer("\"some\nstring\"");

            Assert.Equal(TokenType.String, lexer.Tokens.First().Type);
        }
        
        [Fact]
        public void StringWithErrorSymbol()
        {
            var lexer = new Lexer("\"some string\"^");

            Assert.Equal(TokenType.String, lexer.Tokens.First().Type);
            Assert.Equal(TokenType.Error, lexer.Tokens.ToList()[1].Type);
        }
        
        [Fact]
        public void StringWithKeyword()
        {
            var lexer = new Lexer("\"let while string\"^");

            Assert.Equal(TokenType.String, lexer.Tokens.First().Type);
            Assert.Equal(TokenType.Error, lexer.Tokens.ToList()[1].Type);
        }
    }
}