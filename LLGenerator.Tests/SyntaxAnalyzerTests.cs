using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using LLGenerator.TableGenerator;

namespace LLGenerator.Tests
{
    public class SyntaxAnalyzerTests
    {
        [Fact]
        public void FckingTest1()
        {
            var rulesStream = File.OpenRead("../../../test1.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);

            var tableRules = TableGenerator.TableGenerator.Parse(dirRules);
            var input = "Svobodu Keshe Piastri $".Split(" ", StringSplitOptions.TrimEntries);
            var sw = new StringWriter();
            try
            {
                SyntaxAnalyzer.SyntaxAnalyzer.Analyze(input, tableRules);
            }
            catch (Exception ex)
            {
                sw.WriteLine(ex.Message);
                return;
            }

            sw.Write("ura");
            Assert.Equal("ura", sw.ToString());
        }
    }
}