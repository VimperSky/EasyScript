using Xunit;

namespace Lexer.Tests
{
    public class FullTest
    {
        // 1_1 /1 = _ /_1_a /+-1 /2 -1
        // Многострочные комментарии
        // 1.1.1 
        // .1.
        // some"s"string
        // Длина number фиксированна 
        // фикс Error
        // Выводить EOF
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer(
                "fun rofl(a: number, b: number)\nreturn a + b;\nlet str = \"hello world\";\nprints(str);for (let i = 0; i < 2; i++) {\nif (i => 1) {\nprint(i)\n}\n");
        }
    }
}