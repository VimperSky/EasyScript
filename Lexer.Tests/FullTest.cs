using Lexer.Types;
using Xunit;

namespace Lexer.Tests
{
    public class FullTest
    {
        // Многострочные комментарии
        // some"s"string
        // Длина number фиксированна 
        // неправильная position после скобок (всех)
        // фикс Error(position)
        // Выводить EOF при пустом файле
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer(
                "fun sum(number a, number br)\n  return a + br;\n \nconst maxNum = 41;\nfor (let i = 0; i < maxNum; i++) // печать чётных чисел\nif (i / 2 == 0)\nprint(\"Even\");\nlet firstNumber = read(); // read\nlet firstSum = sum(firstNumber + maxNum);\nlet _str = \"Hello\";\nprints(_str);");
            
            Assert.Equal(new Token(TokenType.KeyWord, "fun", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "sum", 0, 5).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenBracket, "(", 0, 8).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "number", 0, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "a", 0, 16).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Separator, ",", 0, 17).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "number", 0, 19).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "br", 0, 26).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Separator, ";", 0, 28).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "return", 1, 2).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "a", 1, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.PlusOp, "+", 1, 11).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "br", 1, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Separator, ";", 1, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "const", 3, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "maxNum", 3, 6).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 3, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Number, "43", 3, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Separator, ";", 3, 17).ToString(), lexer.GetNextToken().ToString());
        }
    }
}