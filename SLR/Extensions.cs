using System.Collections.Generic;
using System.Linq;

namespace SLR
{
    public static class Extensions
    {
        // Without R
        private const string Alphabet = "ABCDEFGHIJKLMNOPQSTUVWXYZ";
        
        public static string GetNextFreeLetter(HashSet<string> takenLetters)
        {
            var freeLetters = Alphabet.ToList();
            freeLetters.RemoveAll(x => takenLetters.Contains(x.ToString()));
            return freeLetters.First().ToString();
        }
    }
}