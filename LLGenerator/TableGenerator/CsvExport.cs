﻿#nullable enable
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using LLGenerator.Entities;

namespace LLGenerator.TableGenerator
{
    public static class CsvExport
    {
        public static void SaveToCsv(IEnumerable<TableRule> rules, IEnumerable<string> input)
        {
            var newList = rules.Select(r => new RulesForCsvExport
            {
                Id = r.Id,
                GoTo = r.GoTo == null ? "NULL" : r.GoTo.Value.ToString(),
                NonTerminal = r.NonTerminal,
                IsError = r.IsError ? "1" : "0",
                IsShift = r.IsShift ? "1" : "",
                MoveToStack = r.MoveToStack ? "1" : "",
                IsEnd = r.IsEnd ? "1" : "",
                Dirs = string.Join(", ", r.DirSet)
            }).ToList();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {Delimiter = ";"};
            using var writer = new StreamWriter("table.csv");
            using var csv = new CsvWriter(writer, config);
            csv.WriteHeader<RulesForCsvExport>();
            csv.NextRecord();
            foreach (var rule in newList)
            {
                csv.WriteRecord(rule);
                csv.NextRecord();
            }

            //csv.NextRecord();
            //foreach (var field in input) csv.WriteField(field);
        }

        private struct RulesForCsvExport
        {
            public int Id { get; init; }
            public string NonTerminal { get; init; }
            public string Dirs { get; init; }
            public string GoTo { get; init; }
            public string IsError { get; init; }
            public string IsShift { get; init; }
            public string MoveToStack { get; init; }
            public string IsEnd { get; init; }

            public override string ToString()
            {
                return $"Id: {Id}, NonTerm: {NonTerminal}, Dirs: {Dirs}, " +
                       $"Goto: {GoTo}, Err: {IsError}, " +
                       $"Shift: {IsShift}, Stack: {MoveToStack}, End: {IsEnd}";
            }
        }
    }
}