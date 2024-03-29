﻿
using Lexer.Types;

namespace SymbolTable;

public class SymbolTableProcessor
{
    private readonly Stack<Stack<TableItem>> _tables;
    private Stack<TableItem> TopTable => _tables.Peek();

    public SymbolTableProcessor()
    {
        _tables = new Stack<Stack<TableItem>>();
        CreateTable();
    }
    
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
        if (TopTable.Any(x => x.Name == name))
            throw new ArgumentException($"[SymbolTable] Trying to push token with an existing name: {name}");
        
        TopTable.Push(new TableItem {TokenType = type, Name = name});
    }

    public TokenType? Get(string name)
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