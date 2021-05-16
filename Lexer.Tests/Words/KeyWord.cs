using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Words
{
    public class KeyWord
    {
        private readonly TokenType[] _tokenTypes =
        {
            TokenType.Ask, TokenType.Askl, TokenType.Const, TokenType.Else, TokenType.For, TokenType.Fun, TokenType.If,
            TokenType.Say, TokenType.Sayl, TokenType.While
        };

        [Fact]
        public void DefaultKeywords()
        {
            var lexer = new TestLexer("ask askl const else for fun if say sayl while");

            foreach (var token in _tokenTypes) Assert.Equal(token, lexer.GetNextToken().Type);
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