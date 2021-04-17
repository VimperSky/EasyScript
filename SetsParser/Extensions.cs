using System.Linq;

namespace SetsParser
{
    public static class Extensions
    {
        public static bool IsNonTerminal(string str)
        {
            return str.All(char.IsUpper);
        }
    }
}