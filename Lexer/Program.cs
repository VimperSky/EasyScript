using System;
using System.IO;

namespace Lexer;

internal static class Program
{
    private static void Main(string[] args)
    {
        var lexer = new Lexer(File.OpenRead("input.txt"));
        foreach (var token in lexer.Tokens) Console.WriteLine(token);
    }
}