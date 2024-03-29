﻿using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Types
{
    public class Number
    {
        [Fact]
        public void DefaultNumber()
        {
            var lexer = new TestLexer("5");

            Assert.Equal(TokenType.AnyInt, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithMultipleCharacters()
        {
            var lexer = new TestLexer("52");

            Assert.Equal(TokenType.AnyInt, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithFloat()
        {
            var lexer = new TestLexer("5.222");

            Assert.Equal(TokenType.AnyFloat, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithoutIntPart()
        {
            var lexer = new TestLexer(".222");

            Assert.Equal(TokenType.AnyFloat, lexer.GetNextToken().Type);
        }

        [Fact]
        public void NumberWithoutFractionalPart()
        {
            var lexer = new TestLexer("5.");

            Assert.Equal(TokenType.AnyFloat, lexer.GetNextToken().Type);
        }

        [Fact]
        public void StartAndEndWithDot()
        {
            var lexer = new TestLexer(".1.");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }

        [Fact]
        public void ManyDots()
        {
            var lexer = new TestLexer("1.1.1");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }

        [Fact]
        public void ManyDotsWithLetter()
        {
            var lexer = new TestLexer("1.1.a");

            Assert.Equal(TokenType.Error, lexer.GetNextToken().Type);
        }
    }
}