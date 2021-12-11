using Lexer.Types;

namespace SymbolTable;

internal class TableItem
{
    public TokenType TokenType { get; init; }
    public string Name { get; init; }
}