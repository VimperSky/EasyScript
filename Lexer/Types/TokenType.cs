namespace Lexer.Types
{
    public enum TokenType
    {
        Space,
        EndLine,

        // Ошибка
        Error,

        // Обычные скоюбки
        OpenBracket, // (
        CloseBracket, // )

        // Фигурные скоюки
        OpenBrace, // {
        CloseBrace, // }

        // Комментарий
        SingleComment, // //
        MultiLineComment, // /* */

        // Ключевое слово (let, if, for ...)
        KeyWord,

        // Название переменной
        Identifier,

        // Типы данных
        Int, // 32 bit integer
        Float, // 32 bit floating point
        String, // "

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
        
        EoF, // End of Line
    }
}