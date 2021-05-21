using System;
using System.IO;
using SLR.Types;

namespace SLR
{
    class Program
    {
        static void Main(string[] args)
        {
            var rules = SimpleRulesParser.Parse(File.OpenRead("rules.txt"));
            Console.WriteLine(rules);
        }
    }
}