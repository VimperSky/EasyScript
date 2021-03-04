using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Words
{
    public class KeyWord
    {
        [Fact]
        public void Let()
        {
            var lexer = new TestLexer("let");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void For()
        {
            var lexer = new TestLexer("for");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void While()
        {
            var lexer = new TestLexer("while");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void Fun()
        {
            var lexer = new TestLexer("fun");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void Print()
        {
            var lexer = new TestLexer("print");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void Read()
        {
            var lexer = new TestLexer("read");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void If()
        {
            var lexer = new TestLexer("if");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void Else()
        {
            var lexer = new TestLexer("else");

            Assert.Equal(TokenType.KeyWord, lexer.GetNextToken().Type);
        }

        [Fact]
        public void KeywordInIdentifier()
        {
            var lexer = new TestLexer("reading letter");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }
    }
}