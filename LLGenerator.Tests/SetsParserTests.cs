using System.IO;
using System.Linq;
using Xunit;

namespace LLGenerator.Tests
{
    public class SetsParserTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        [InlineData("7")]
        [InlineData("8")]
        [InlineData("9")]
        [InlineData("10")]
        [InlineData("11")]
        [InlineData("12")]
        [InlineData("13")]
        [InlineData("14")]
        [InlineData("15")]
        [InlineData("16")]
        [InlineData("17")]
        [InlineData("18")]
        [InlineData("19")]
        [InlineData("20")]
        public void RunTests(string id)
        {
            var input = File.OpenRead($"../../../TestCases/{id}.test");
            var dirRules = Program.DoParse(input);
            var expected = File.ReadAllLines($"../../../Expected/{id}.test");
            Assert.Equal(expected, dirRules.Select(x => x.ToString()));
        }
    }
}