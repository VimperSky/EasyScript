using System.IO;
using System.Linq;

namespace Generator.InputParsing
{
    public class LexerInputParser: IInputParser
    {
        private readonly string _path;

        public LexerInputParser(string path)
        {
            _path = path;
        }

        public string[] Parse()
        {
            var lexer = new Lexer.Lexer(File.OpenRead(_path));
            return lexer.Tokens.Select(x => x.ToString()).ToArray();
        }
    }
}