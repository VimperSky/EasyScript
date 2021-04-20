﻿using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using LLGenerator.Entities;
using LLGenerator.SetsParser;
using LLGenerator.TableGenerator;

namespace TestSolver
{
    internal static class Program
    {
        private static void Main()
        {
            var dirRules = SetsParser.DoParse(File.OpenRead("input.txt"));
            foreach (var rule in dirRules) Console.WriteLine(rule);

            var tableRules = TableGenerator.Parse(dirRules);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {Delimiter = ";"};
            using var writer = new StreamWriter("table.csv");
            using var csv = new CsvWriter(writer, config);
            csv.WriteHeader<TableRule>();
            csv.NextRecord();
            foreach (var rule in tableRules)
            {
                csv.WriteRecord(rule);
                csv.NextRecord();
            }
        }
    }
}