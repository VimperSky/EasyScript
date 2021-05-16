using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Types
{
    public class Bool
    {
        [Fact]
        public void DefaultBool()
        {
            var lexer = new TestLexer("true false");

            Assert.Equal(TokenType.True, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.False, lexer.GetNextToken().Type);
        }

        [Fact]
        public void BoolAfterIdentifier()
        {
            var lexer = new TestLexer("some false");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.False, lexer.GetNextToken().Type);
        }

        [Fact]
        public void BoolBeforeIdentifier()
        {
            var lexer = new TestLexer("false some");

            Assert.Equal(TokenType.False, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }
    }
}