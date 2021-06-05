﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generator.RulesParsing
{
    public class TxtRulesParser: IRulesParser
    {
        public List<(string NonTerminal, string RightBody)> Parse(Stream stream)
        {
            using var sr = new StreamReader(stream);
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