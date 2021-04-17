using System;
using System.IO;

namespace SetsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var lexer = new SetsParser(File.OpenRead("input.txt"));
        }
    }
}