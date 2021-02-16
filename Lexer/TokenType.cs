namespace Lexer
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
        Comment, // //

        // Ключевое слово (let, if, for ...)
        KeyWord,

        // Название переменной
        Identifier,

        // Типы данных
        Number, // 
        String, // "

        // Точка с запятой
        Separator, // ;

        // Присвоение
        Assign, // =

        // Математические операторы
        MinusOp, // +
        PlusOp, // -
        DivOp, // / 
        MultiplyOp, // *

        // Операторы сравнения
        MoreEquals, // >=
        LessEquals, // <=
        More, // >
        Less, // < 
        Equals, // ==
        NotEquals, // !=

        // Операторы условия
        And, // &
        Or // |
    }
}