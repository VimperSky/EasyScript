﻿using System;
using System.IO;

namespace SLR
{
    class Program
    {
        static void Main(string[] args)
        {
            var rules = SimpleRulesParser.Parse(File.OpenRead("rules.txt"));
            
            var tableBuilder = new TableBuilder(rules);
            tableBuilder.CreateTable();
        }
    }
}