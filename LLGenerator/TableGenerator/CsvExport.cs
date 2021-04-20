using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using LLGenerator.Entities;

namespace LLGenerator.TableGenerator
{
    public class CsvExport
    {
        public class RulesForCsvExport
                {
                    public int Id { get; init; }
                    public string NonTerminal { get; init; }
                    public int? GoTo { get; set; }
                    public bool IsError { get; init; }
                    public bool IsShift { get; init; }
                    public bool MoveToStack { get; init; }
                    public bool IsEnd { get; init; }
                    public string Dirs { get; init; }
        
                    public override string ToString()
                    {
                        return $"Id: {Id}, NonTerm: {NonTerminal}, Dirs: {Dirs}, " +
                               $"Goto: {(GoTo == null ? "NULL" : GoTo)}, Err: {IsError}, " +
                               $"Shift: {IsShift}, Stack: {MoveToStack}, End: {IsEnd}";
                    }
                }

        public static void SaveToCsv(List<TableRule> rules)
                {
                    var newList = rules.Select(r => new RulesForCsvExport
                        {
                            Id = r.Id,
                            GoTo = r.GoTo,
                            NonTerminal = r.NonTerminal,
                            IsError = r.IsError,
                            IsShift = r.IsShift,
                            MoveToStack = r.MoveToStack,
                            IsEnd = r.IsEnd,
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
                }
    }
}