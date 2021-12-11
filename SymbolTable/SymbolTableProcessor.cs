
using Lexer.Types;

namespace SymbolTable;

public class SymbolTableProcessor
{
    private readonly Stack<Stack<TableItem>> _tables = new();
    private Stack<TableItem> TopTable => _tables.Peek();

    public void CreateTable()
    {
        _tables.Push(new Stack<TableItem>());
    }

    public void DestroyTable()
    {
        _tables.Pop();
    }

    public void Push(TokenType type, string name)
    {
        TopTable.Push(new TableItem {TokenType = type, Name = name});
    }

    public TokenType? Find(string name)
    {
        foreach (var item in _tables)
        {
            var value = item.FirstOrDefault(x => x.Name == name);
            if (value != null)
                return value.TokenType;
        }

        return null;
    }
}