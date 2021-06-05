using System.IO;

namespace Generator.InputParsing
{
    public interface IInputProcessor
    {
        string[] Parse(Stream stream);
    }
}