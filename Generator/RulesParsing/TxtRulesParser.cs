using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generator.RulesParsing
{
    public class TxtRulesParser: IRulesParser
    {
        private readonly string _path;
        public TxtRulesParser(string path)
        {
            if (!path.EndsWith(".txt"))
                throw new ArgumentException("TXT Rules Parser accepts only txt files!");
            
            _path = path;
        }
        
        public List<(string NonTerminal, string RightBody)> Parse()
        {
            using var sr = new StreamReader(_path);
            string line;
            var rawRules = new List<(string LeftBody, string RightBody)>();
            while ((line = sr.ReadLine()) != null)
            {
                var split = line.Split("->", StringSplitOptions.TrimEntries);
                var localRules = split[1].Split("|", StringSplitOptions.TrimEntries);
                rawRules.AddRange(localRules.Select(rule => (split[0], rule)));
            }

            return rawRules;
        }
    }
}