using Lexer.Types;
using Xunit;

namespace Lexer.Tests.SeparatorsAndComments
{
    public class RoundBracket
    {
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer("()");

            Assert.Equal(TokenType.OpenBracket, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.CloseBracket, lexer.GetNextToken().Type);
        }

        [Fact]
        public void BracketsInIdentifier()
        {
            var lexer = new TestLexer("lost)s");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.CloseBracket, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }

        [Fact]
        public void BracketsInComment()
        {
            var lexer = new TestLexer("// maybe this (or not)");

            Assert.Equal(TokenType.Comment, lexer.GetNextToken().Type);
        }
    }
}