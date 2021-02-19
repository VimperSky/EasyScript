using System.Linq;
using Xunit;
using Lexer.States;

namespace Lexer.Tests
{
    public class Number
    {
        
        [Fact]
        public void DefaultNumber()
        {
            var machine = new LexerMachine(); 
            Extension.ProcessString("5", machine);

            Assert.Equal(TokenType.Number, machine.GetTokens().First().Type);
        }
        
        [Fact]
        public void NumberWithMultipleCharacters()
        {
            var machine = new LexerMachine(); 
            Extension.ProcessString("52", machine);

            Assert.Equal(TokenType.Number, machine.GetTokens().First().Type);
        }
        
        [Fact]
        public void NumberWithFloat()
        {
            var machine = new LexerMachine(); 
            Extension.ProcessString("5.222", machine);

            Assert.Equal(TokenType.Number, machine.GetTokens().First().Type);
        }
        
        [Fact]
        public void NegativeNumber()
        {
            var machine = new LexerMachine(); 
            Extension.ProcessString("-5.222", machine);

            Assert.Equal(TokenType.Number, machine.GetTokens().First().Type);
        }
        
        [Fact]
        public void NegativeNumberAfterNumber()
        {
            var machine = new LexerMachine(); 
            Extension.ProcessString("2 -5.222", machine);

            Assert.Equal(TokenType.Number, machine.GetTokens().First().Type);
        }
    }
}