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
            const string str = "\"some string\"";
            var machine = new LexerMachine();
            foreach (var t in str)
            {
                machine.ProcessChar(t, 0, 0);
            }

            Assert.Equal(machine.GetTokens().First().Type, TokenType.String);
            
        }
    }
}