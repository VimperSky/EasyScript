using Xunit;

namespace Lexer.Tests
{
    public class FullTest
    {
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer(
                "fun rofl(a: number, b: number)\nreturn a + b;\nlet str = \"hello world\";\nprints(str);for (let i = 0; i < 2; i++) {\nif (i => 1) {\nprint(i)\n}\n");
        }
    }
}