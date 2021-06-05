using System.IO;
using System.Linq;

namespace Generator.InputParsing
{
    public class SimpleRulesProcessor: IInputProcessor
    {
        public string[] Parse(Stream stream)
        {
            var sw = new StreamReader(stream);
            var text = sw.ReadToEnd();
            return text.Split(" ").ToArray();
        }
    }
}