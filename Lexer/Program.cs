using System.IO;

namespace Lexer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var sr = new StreamReader("input.txt");

            var lexer = new Lexer(sr);
            lexer.Run(sr);
        }
    }
}