using Lexer.Types;
using Xunit;

namespace Lexer.Tests.SeparatorsAndComments
{
    public class Comments
    {
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer("//");

            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void CommentInComment()
        {
            var lexer = new TestLexer("// //");

            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
            Assert.Null(lexer.GetNextToken());
        }

        [Fact]
        public void CommentWithKeywords()
        {
            var lexer = new TestLexer("// let this");

            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void CommentsInAnotherString()
        {
            var lexer = new TestLexer("// let this\n get the fo // sry for this)");

            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void MultilineComments()
        {
            var lexer = new TestLexer("/* lol try this\non another string\n*/");

            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void MultilineCommentsInMoreThanOneStrings()
        {
            var lexer = new TestLexer("/* lol try this\non another\nstring*/");

            Assert.Equal(TokenType.SingleComment, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NotClosedMultilineComments()
        {
            var lexer = new TestLexer("/* lol try this\n");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NotClosedMultilineCommentsWithAnotherStrings()
        {
            var lexer = new TestLexer("/* lol try this\non another");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
        }
    }
}