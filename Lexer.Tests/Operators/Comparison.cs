using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Operators
{
    public class Comparison
    {
        [Fact]
        public void More()
        {
            var lexer = new TestLexer(">");

            Assert.Equal(TokenType.More, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void MoreEquals()
        {
            var lexer = new TestLexer(">=");

            Assert.Equal(TokenType.MoreEquals, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void Less()
        {
            var lexer = new TestLexer("<");

            Assert.Equal(TokenType.Less, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void LessEquals()
        {
            var lexer = new TestLexer("<=");

            Assert.Equal(TokenType.LessEquals, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void Equal()
        {
            var lexer = new TestLexer("==");

            Assert.Equal(TokenType.Equals, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void NotEqual()
        {
            var lexer = new TestLexer("!=");

            Assert.Equal(TokenType.NotEquals, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void ManyCompOp()
        {
            var lexer = new TestLexer("!= == <= >= < >");

            Assert.Equal(TokenType.NotEquals, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Equals, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.LessEquals, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.MoreEquals, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Less, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.More, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void DefaultOperation()
        {
            var lexer = new TestLexer("assign != 3");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.NotEquals, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }
    }
}