using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SetsParser
{
    public class SetsParser
    {
        private readonly StreamReader _streamReader;

        public SetsParser(Stream input)
        {
            _streamReader = new StreamReader(input);
            string line;
            var rules = new List<Rule>();
            while ((line = _streamReader.ReadLine()) != null)
            {
                var terminals = line.Split("->");
                var right = terminals[1].Split("|");
                rules.AddRange(right.Select(item => new Rule
                {
                    NonTerminal = terminals[0],
                    Elements = item.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()
                }));
            }
        }
    }
}