using Lexer.Types;
using Xunit;

namespace Lexer.Tests
{
    public class FullTest
    {
        // 1_1 /1 = _ /_1_a /+-1 /2 -1 - done
        // Многострочные комментарии - done
        // 1.1.1 - done
        // .1. - done
        // some"s"string - done
        // Длина number фиксированна - done 
        // фикс Error - done
        // Выводить EOF - нет смысла
        // err position - done
        [Fact]
        public void Default()
        {
            var lexer = new TestLexer(
                "fun sum(number a, number br)\n  return a + br;\n \nconst maxNum = 41;\nfor (let i =0; i <= maxNum; i++) " + 
                "//печать чётных чисел\nif (i / 2 == 0)\nprint(\"Even\");\nlet firstNumber = read(); // read\n" + 
                "let firstSum = sum(firstNumber + maxNum);\nlet _str = \"Hello\";\nprints(_str);");

            Assert.Equal(new Token(TokenType.KeyWord, "fun", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "sum", 0, 5).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenBracket, "(", 0, 8).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "number", 0, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "a", 0, 16).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ",", 0, 17).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "number", 0, 19).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "br", 0, 26).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 0, 28).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "return", 1, 2).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "a", 1, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.PlusOp, "+", 1, 11).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "br", 1, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 1, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "const", 3, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "maxNum", 3, 6).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 3, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Int, "43", 3, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 3, 17).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "for", 4, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenBrace, "(", 4, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.KeyWord, "let", 4, 5).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 4, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 4, 11).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Int, "0", 4, 12).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 4, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 4, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.LessEquals, "<=", 4, 17).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "maxNum", 4, 20).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 4, 26).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 4, 28).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Increment, "++", 4, 29).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseBracket, ")", 4, 31).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Comment, "печать чётных чисел", 4, 33).ToString(),lexer.GetNextToken().ToString());
        }
    }
}