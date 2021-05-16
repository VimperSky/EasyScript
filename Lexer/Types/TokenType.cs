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
        
        // Ключевые слово (let, if, for ...)
        Const = 100,
        Let = 101,
        If = 102,
        Else = 103,
        For = 104,
        While = 105,
        True = 106,
        False = 107,
        Say = 108,
        Says = 109,
        Ask = 110,
        Asks = 111,
        Fun = 112,
        Ret = 113,
        Int = 114,
        Float = 115,
        Bool = 116,
        String = 117,
    }
}