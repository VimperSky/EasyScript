using System.Linq;
using Xunit;
using Lexer.States;

namespace Lexer.Tests
{
    public class String
    {
        [Fact]
        public void DefaultString()
        {
            var machine = new LexerMachine(); 
            Extension.ProcessString("\"some string\"", machine);

            Assert.Equal(TokenType.String, machine.GetTokens().First().Type);
        }
        
        [Fact]
        public void StringWithNewLine()
        {
            var machine = new LexerMachine();
            Extension.ProcessString("\"some\nstring\"", machine);

            Assert.Equal(TokenType.String, machine.GetTokens().First().Type);
        }
        
        [Fact]
        public void StringWithErrorSymbol()
        {
            var machine = new LexerMachine();
            Extension.ProcessString("\"some string\"^", machine);

            Assert.Equal(TokenType.String, machine.GetTokens().First().Type);
            Assert.Equal(TokenType.Error, machine.GetTokens().ToList()[1].Type);
        }
        
        [Fact]
        public void StringWithKeyword()
        {
            var machine = new LexerMachine();
            Extension.ProcessString("\"let while string\"^", machine);

            Assert.Equal(TokenType.String, machine.GetTokens().First().Type);
            Assert.Equal(TokenType.Error, machine.GetTokens().ToList()[1].Type);
        }
    }
}