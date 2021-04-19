using System.Collections.Generic;
using CsvHelper.Configuration;

namespace Common
{
    public class TableRuleClassMap : ClassMap<TableRule>
    {
        public TableRuleClassMap()
        {
            Map(x => x.Id).Name("ID");
            Map(x => x.NonTerminal).Name("NonTerminal");
            Map(x => x.DirSet).Name("DirSet");
            Map(x => x.GoTo).Name("GoTo");
            Map(x => x.IsError).Name("ERR");
            
        }
    }
    public class TableRule
    {
        public int Id { get; init; }
        public string NonTerminal { get; init; }
        public HashSet<string> DirSet { get; init; }
        public int? GoTo { get; set; }
        public bool IsError { get; init; }

        public override string ToString()
        {
            return $"{Id}   {NonTerminal}   {string.Join(", ", DirSet)} {GoTo} {IsError}";
        }
    }
}