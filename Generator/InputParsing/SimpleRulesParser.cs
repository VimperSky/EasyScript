using System.IO;
using System.Linq;

namespace Generator.InputParsing
{
    public class SimpleRulesParser : IInputParser
    {
        private readonly string _path;

        public SimpleRulesParser(string path)
        {
            _path = path;
        }

        public string[] Parse()
        {
            var sw = new StreamReader(_path);
            var text = sw.ReadToEnd();
            return text.Split(" ").Append(Constants.EndSymbol).ToArray();
        }
    }
}