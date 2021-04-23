using System.IO;
using Xunit;

namespace LLGenerator.Tests
{
    public class SetsParserTests
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
                "P1 -> Day P2 Keshe $ / Day\r\nP1 -> P2 $ / Svobodu, Piastri, Nu\r\nP2 -> Svobodu A / Svobodu\r\n" +
                "P2 -> P3 A / Piastri, Nu\r\nA -> Keshe P3 A / Keshe\r\nA -> e / Keshe, $\r\n" +
                "P3 -> Piastri B / Piastri\r\nP3 -> Nu P4 Ti B / Nu\r\nB -> Kashi B / Kashi\r\nB -> e / Keshe, $\r\n" +
                "P4 -> oh P1 ugass / oh\r\nP4 -> e / Ti\r\n";

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
                "F -> function I ( I ) G end $ / function\r\nG -> I := E B / a\r\nB -> ; G / ;\r\nB -> e / end\r\n" +
                "E -> I A / a\r\nA -> * I A / *\r\nA -> + I A / +\r\nA -> e / ;, end\r\nI -> a / a\r\n";
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void FckingTest3()
        {
            var rulesStream = File.OpenRead("../../../test3.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "F -> S1 $ / f, a\r\nS1 -> f C / f\r\nC -> S1 / f, a\r\nC -> A / c\r\nC -> a / a\r\nS1 -> a D / a\r\n" +
                "D -> A / c\r\n" + "D -> d / d\r\nA -> c B / c\r\nB -> a B / a\r\nB -> b B / b\r\nB -> e / $\r\n";
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void FckingTest4()
        {
            var rulesStream = File.OpenRead("../../../test4.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "F -> S $ / x\r\nS -> x B / x\r\nB -> A B / y\r\nB -> e / $, x, y\r\nA -> y C / y\r\n" +
                "C -> S C / x\r\nC -> e / y, $, x\r\n";
            Assert.Equal(expected, sw.ToString());
        }

        [Fact]
        public void FckingTest5()
        {
            var rulesStream = File.OpenRead("../../../test5.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "F -> S $ / (, b\r\nS -> ( S ) A / (\r\nS -> b A / b\r\nA -> a A / a\r\nA -> e / $, )\r\n";
            Assert.Equal(expected, sw.ToString());
        }

        /*F -> S $
        S -> S a | S b | ( S ) | e
        
        F -> S $ / (, a, b, $, )
        S -> ( S ) A / (
        S -> A / a, b, $, )
        A -> a A / a
        A -> b A / b
        A -> e / $, )
        */
        [Fact]
        public void FckingTest6()
        {
            var rulesStream = File.OpenRead("../../../test6.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "F -> S $ / (, a, b, $, )\r\nS -> ( S ) A / (\r\nS -> A / a, b, $, )\r\nA -> a A / a\r\nA -> b A / b\r\nA -> e / $, )\r\n";
            Assert.Equal(expected, sw.ToString());
        }
        
        /*F -> S $
        S -> id = E | while E do S
        E -> E + E | id
        
        F -> S $ / id, while
        S -> id = E / id
        S -> while E do S / while
        E -> id A / id
        A -> + E A / +
        A -> e / $, do, +
        */
        [Fact]
        public void FckingTest7()
        {
            var rulesStream = File.OpenRead("../../../test7.txt");
            var dirRules = SetsParser.SetsParser.DoParse(rulesStream);
            var sw = new StringWriter();
            foreach (var rule in dirRules)
                sw.WriteLine(rule);
            const string expected =
                "F -> S $ / id, while\r\nS -> id = E / id\r\nS -> while E do S / while\r\n" + 
                "E -> id A / id\r\nA -> + E A / +\r\nA -> e / $, do, +\r\n";
            Assert.Equal(expected, sw.ToString());
        }
    }
}