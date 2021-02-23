using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Words
{
    public class Identifier
    {
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer("lexical");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void IdentifierWithNumbers()
        {
            var lexer = new TestLexer("lexic4l");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void IdentifierStartAtDigit()
        {
            var lexer = new TestLexer("0flex");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void AnotherSymbolInIdentifier()
        {
            var lexer = new TestLexer("Fle$x");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }       
        
        [Fact]
        public void StringInIdentifier()
        {
            var lexer = new TestLexer("flex\"path\"er");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }
    }
}