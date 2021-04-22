using System.IO;
using Xunit;

namespace LLGenerator.Tests
{
    public class Factorization
    {
        [Fact]
        public void FckingTest1()
        {
            var rulesStream = File.OpenRead("../../../test1.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "P1 -> Day P2 Keshe $ / Day\r\nP1 -> P2 $ / Svobodu, Piastri, Nu\r\nP2 -> Svobodu A / Svobodu\r\nP2 -> P3 A / Piastri, Nu\r\nA -> Keshe P3 A / Keshe\r\nA -> e / Keshe, $\r\nP3 -> Piastri B / Piastri\r\nP3 -> Nu P4 Ti B / Nu\r\nB -> Kashi B / Kashi\r\nB -> e / Keshe, $\r\nP4 -> oh P1 ugass / oh\r\nP4 -> e / Ti\r\n";

            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void FckingTest2()
        {
            var rulesStream = File.OpenRead("../../../test2.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "F -> function I ( I ) G end $ / function\r\nG -> I := E B / a\r\nB -> ; G / end\r\nB -> e / ;\r\nE -> I A / a\r\nB -> * I A / *\r\nA -> + I A / +\r\nA -> e / end, ;\r\nI -> a / a\r\n";
            Assert.Equal(expected, sw.ToString());
        }
    }
}