using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using SLR.Types;

namespace SLR.Table
{
    public static class CsvExport
    {
        public static void SaveToCsv(IList<TableRule> rules)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", 
                SanitizeForInjection = true
            };
            
            using var csv = new CsvWriter(new StreamWriter("table.csv"), config);
            csv.WriteField("");
            foreach (var item in rules.First().Values) csv.WriteField(item.Key);
            csv.NextRecord();
            foreach (var rule in rules)
            {
                csv.WriteField(rule.Key);
                foreach (var value in rule.Values) csv.WriteField(value.Value.ToString());

                csv.NextRecord();
            }
        }
    }
}