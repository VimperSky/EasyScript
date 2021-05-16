using System;

namespace Lexer.Types
{
    public enum TokenType: byte
    {
        Space,
        EndLine,
        EoF,

        // Ошибка
        Error,

        // Обычные скоюбки
        OpenBracket, // (
        CloseBracket, // )

        // Фигурные скобки
        OpenBrace, // {
        CloseBrace, // }

        // Комментарии
        Comment, // //
        MultiComment, // /* */

        // Название переменной
        Identifier,

        // Типы данных
        AnyInt, // 32 bit integer
        AnyFloat, // 32 bit floating point
        AnyString, // "

        // Точка с запятой
        Semicolon, // ;
        Comma, // ,

        // Присвоение
        Assign, // =

        // Математические операторы
        MinusOp, // +
        PlusOp, // -
        DivOp, // / 
        MultiplyOp, // *

        Increment, // ++
        Decrement, // --

        // Операторы сравнения
        MoreEquals, // >=
        LessEquals, // <=
        More, // >
        Less, // < 
        Equals, // ==
        NotEquals, // !=

        // Операторы условия
        And, // &
        Or, // |
        
        // Ключевые слова
        Const = 100,
        Let,
        If,
        Else,
        For,
        While,
        True,
        False,
        Say,
        Sayl,
        Ask,
        Askl,
        Fun,
        Ret,
        Int,
        Float,
        Bool,
        Str,
    }
}