using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Generator.RulesParsing
{
    public class OnlineCsvRulesParser : IRulesParser
    {
        private const string Uri = "https://docs.google.com/spreadsheets/d/1il400qxgMaQPXHwclYMqB046jTjrcPrhMgwgyQX0pjk/gviz/tq?tqx=out:csv&sheet=ACTUAL";
        private static readonly HttpClient HttpClient = new();
        private static async Task<Stream> DownloadCsv()
        {
            return await HttpClient.GetStreamAsync(Uri);
        }
        
        public List<(string NonTerminal, string RightBody)> Parse()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ","
            };

            var stream = Task.Run(DownloadCsv).GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
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