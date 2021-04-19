using System;
using System.IO;
using LLTableGenerator;

namespace TestSolver
{
    internal static class Program
    {
        private static void Main()
        {
            var inputStream = File.OpenRead("input.txt");
            var dirRules = SetsParser.SetsParser.DoParse(inputStream);
            foreach (var rule in dirRules)
            {
                Console.WriteLine(rule);
            }
            
            var tableRules = Generator.Parse(dirRules);
            foreach (var tableRule in tableRules)
            {
                Console.WriteLine(tableRule);
            }
        }
    }
}