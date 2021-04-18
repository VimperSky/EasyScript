using System.IO;

namespace SetsParser
{
    internal static class Program
    {
        private static void Main()
        {
            var setsParser = new SetsParser(File.OpenRead("input.txt"));
        }
    }
}