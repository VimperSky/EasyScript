using Lexer.Types;
using Xunit;

namespace Lexer.Tests.SeparatorsAndComments
{
    public class CurlyBrace
    {
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer("{}");

            Assert.Equal(TokenType.OpenBrace, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.CloseBrace, lexer.GetNextToken().Type);
        }

        [Fact]
        public void BraceInIdentifier()
        {
            var lexer = new TestLexer("anot}he{r");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.CloseBrace, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.OpenBrace, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }

        [Fact]
        public void BraceInComment()
        {
            var lexer = new TestLexer("// im so tired {of this}");

            Assert.Equal(TokenType.Comment, lexer.GetNextToken().Type);
        }
    }
}