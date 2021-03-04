using Lexer.Types;
using Xunit;

namespace Lexer.Tests.SeparatorsAndComments
{
    public class Semicolon
    {
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer(";");

            Assert.Equal(TokenType.Separator, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInIdentifier()
        {
            var lexer = new TestLexer("so;me");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Separator, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInString()
        {
            var lexer = new TestLexer("\"so;me%^\"");

            Assert.Equal(TokenType.String, lexer.GetNextToken().Type);
        }

        [Fact]
        public void SeparatorInComment()
        {
            var lexer = new TestLexer("// TODO: separators (\';\')");

            Assert.Equal(TokenType.Comment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void DefaultString()
        {
            var lexer = new TestLexer("if; else; flex;");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Separator, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Separator, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Separator, lexer.GetNextToken().Type);
        }
    }
}