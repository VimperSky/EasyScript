using System.IO;

namespace Lexer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using var sr = new StreamReader("input.txt");
            using var sw = new StreamWriter("output.txt");

            var lexer = new Lexer();
            lexer.Run(sr, sw);
        }
    }
}