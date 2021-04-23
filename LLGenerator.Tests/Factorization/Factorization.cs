using System.Collections.Generic;
using System.IO;
using LLGenerator.Entities;
using LLGenerator.SetsParser;
using Xunit;

namespace LLGenerator.Tests.Factorization
{
    public class Factorization
    {
        [Fact]
        public void Title1()
        {
            /*P1 -> Day P2 Keshe $
            P1 -> P2 $
            P2 -> P2 Keshe P3
            P2 -> Svobodu
            P2 -> P3
            P3 -> Piastri | P3 Kashi | Nu P4 Ti
            P4 -> oh P1 ugass
            P4 -> e*/
            var check = new RuleList(new List<Rule>
            {
                new Rule(),
            }, new HashSet<string> { });
            var rulesStream = File.OpenRead("../../../test1.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "P1 -> Day P2 Keshe $ / Day\r\nP1 -> P2 $ / Svobodu, Piastri, Nu\r\nP2 -> Svobodu A / Svobodu\r\n" +
                "P2 -> P3 A / Piastri, Nu\r\nA -> Keshe P3 A / Keshe\r\nA -> e / Keshe, $\r\n" +
                "P3 -> Piastri B / Piastri\r\nP3 -> Nu P4 Ti B / Nu\r\nB -> Kashi B / Kashi\r\nB -> e / Keshe, $\r\n" +
                "P4 -> oh P1 ugass / oh\r\nP4 -> e / Ti\r\n";

            Assert.Equal(expected, sw.ToString());
        }
    }
}