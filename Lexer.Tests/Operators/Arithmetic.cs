﻿using Lexer.Types;
using Xunit;

namespace Lexer.Tests.Operators
{
    public class Arithmetic
    {
        [Fact]
        public void Plus()
        {
            var lexer = new TestLexer("+");

            Assert.Equal(TokenType.PlusOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void Minus()
        {
            var lexer = new TestLexer("-");

            Assert.Equal(TokenType.MinusOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void Divide()
        {
            var lexer = new TestLexer("/");

            Assert.Equal(TokenType.DivOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void Multiply()
        {
            var lexer = new TestLexer("*");

            Assert.Equal(TokenType.MultiplyOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void ArithmeticOpBeforeWord()
        {
            var lexer = new TestLexer("some*");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.MultiplyOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void ArithmeticOpAfterWord()
        {
            var lexer = new TestLexer("some*");

            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.MultiplyOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void ArithmeticOpAfterWordAndSpace()
        {
            var lexer = new TestLexer("some *");

            
            Assert.Equal(TokenType.Identifier, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.MultiplyOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void ManyArithmeticOp()
        {
            var lexer = new TestLexer("-+*/");

            Assert.Equal(TokenType.MinusOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.PlusOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.MultiplyOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.DivOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void ManyArithmeticOpWithSpaces()
        {
            var lexer = new TestLexer("- + * /");

            Assert.Equal(TokenType.MinusOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.PlusOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.MultiplyOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.DivOp, lexer.GetNextToken().Type);
        }
        
        [Fact]
        public void DefaultOperation()
        {
            var lexer = new TestLexer("2 + 2");

            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.PlusOp, lexer.GetNextToken().Type);
            Assert.Equal(TokenType.Number, lexer.GetNextToken().Type);
        }
    }
}