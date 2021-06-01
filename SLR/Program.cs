using System.IO;

namespace SLR
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var rules = SimpleRulesParser.Parse(File.OpenRead("rules.txt"));

            var tableBuilder = new TableBuilder(rules);
            tableBuilder.CreateTable();
        }
    }
}