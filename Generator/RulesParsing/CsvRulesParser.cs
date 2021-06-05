using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace Generator.RulesParsing
{
    public class CsvRulesParser: IRulesParser
    {
        private readonly string _path;
        public CsvRulesParser(string path)
        {
            if (!path.EndsWith(".csv"))
                throw new ArgumentException("CSV Rules Parser accepts only csv files!");
            
            _path = path;
        }
        
        public List<(string NonTerminal, string RightBody)> Parse()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            using var reader = new StreamReader(_path);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Record>().ToList();
            return records.Select(record => (record.NonTerminal, record.RightBody)).ToList();
        }

        private class Record
        {
            public string NonTerminal { get; set; }

            public string RightBody { get; set; }
        }
    }
}