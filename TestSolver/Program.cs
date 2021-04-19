using System;
using System.IO;

namespace TestSolver
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var inputStream = File.OpenRead("input.txt");
            var dirRules = SetsParser.SetsParser.DoParse(inputStream);
            foreach (var rule in dirRules)
            {
                Console.WriteLine(rule);
            }
        }
    }
}