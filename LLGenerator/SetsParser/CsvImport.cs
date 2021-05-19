using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace LLGenerator.SetsParser
{
    internal class CsvImport
    {
        private class Record
        {   
            public string NonTerminal { get; set; }
            
            public string RightBody { get; set; }
        }
        
        public static List<(string NonTerminal, string RightBody)> Parse(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Record>().ToList();
            return records.Select(record => (record.NonTerminal, record.RightBody)).ToList();
        }
    }
}