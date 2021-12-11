using Lexer.Types;
using Xunit;

namespace Lexer.Tests
{
    public class FullTest
    {
        [Fact]
        public void Default()
        {
            // fun sum(int a, int br)
            //   ret a + br; 
            //
            // const maxNum = 41; 
            // for (let i =0; i < maxNum; i++) // печать чётных чисел
            //   if (i/ 2 == 0) {
            //     sayl("Even");
            //   }
            // let firstNumber = ask(); // ask
            // let firstSum = sum(firstNumber + maxNum);
            // let _str = "Hello";
            // sayl(_str);
            // /* comment
            // many
            // much */
            // ();
            var lexer = new TestLexer(
                "fun sum(int a, int br)\n  ret a + br;\n\nconst maxNum = 41;\nfor (let i =0; i <= maxNum; i++) " +
                "//печать чётных чисел\n  if (i/ 2 == 0) {\n    say(\"Even\");\n  }\nlet firstNumber = ask(); // ask\n" +
                "let firstSum = sum(firstNumber + maxNum);\nlet _str = \"Hello\";\nsayl(_str);\n/* comment\nmany\nmuch */\n();");

            Assert.Equal(new Token(TokenType.Fun, "fun", 0, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "sum", 0, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 0, 7).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Int, "int", 0, 8).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "a", 0, 12).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Comma, ",", 0, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Int, "int", 0, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "br", 0, 19).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 0, 21).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Ret, "ret", 1, 2).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "a", 1, 6).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.PlusOp, "+", 1, 8).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "br", 1, 10).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 1, 12).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Const, "const", 3, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "maxNum", 3, 6).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 3, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.AnyInt, "41", 3, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 3, 17).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.For, "for", 4, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 4, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Let, "let", 4, 5).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 4, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 4, 11).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.AnyInt, "0", 4, 12).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 4, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 4, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.LessEquals, "<=", 4, 17).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "maxNum", 4, 20).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 4, 26).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 4, 28).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Increment, "++", 4, 29).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 4, 31).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Comment, "печать чётных чисел", 4, 33).ToString(),
                lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.If, "if", 5, 2).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 5, 5).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "i", 5, 6).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.DivOp, "/", 5, 7).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.AnyInt, "2", 5, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Equals, "==", 5, 11).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.AnyInt, "0", 5, 14).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 5, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenBrace, "{", 5, 17).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Say, "say", 6, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 6, 7).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.AnyString, "Even", 6, 8).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 6, 14).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 6, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseBrace, "}", 7, 2).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Let, "let", 8, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "firstNumber", 8, 4).ToString(),
                lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 8, 16).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Ask, "ask", 8, 18).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 8, 21).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 8, 22).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 8, 23).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Comment, " ask", 8, 25).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Let, "let", 9, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "firstSum", 9, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 9, 13).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "sum", 9, 15).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 9, 18).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "firstNumber", 9, 19).ToString(),
                lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.PlusOp, "+", 9, 31).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "maxNum", 9, 33).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 9, 39).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 9, 40).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Let, "let", 10, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "_str", 10, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Assign, "=", 10, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.AnyString, "Hello", 10, 11).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 10, 18).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.Sayl, "sayl", 11, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 11, 4).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Identifier, "_str", 11, 5).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 11, 9).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 11, 10).ToString(), lexer.GetNextToken().ToString());

            Assert.Equal(new Token(TokenType.MultiComment, " comment\nmany\nmuch ", 12, 0).ToString(),
                lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.OpenParenthesis, "(", 15, 0).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.CloseParenthesis, ")", 15, 1).ToString(), lexer.GetNextToken().ToString());
            Assert.Equal(new Token(TokenType.Semicolon, ";", 15, 2).ToString(), lexer.GetNextToken().ToString());
        }
    }
}