using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SLR.Types;

namespace SLR
{
    public static class Extensions
    {
        // Without R
        private const string Alphabet = "ABCDEFGHIJKLMNOPQSTUVWXYZ";

        public static HashSet<string> GetNonTerminals(this ImmutableList<IGrouping<string, Rule>> groups)
        {
            return groups.Select(x => x.Key).ToHashSet();
        }

        public static string GetNextFreeLetter(HashSet<string> takenLetters)
        {
            var freeLetters = Alphabet.ToList();
            freeLetters.RemoveAll(x => takenLetters.Contains(x.ToString()));
            return freeLetters.First().ToString();
        }
    }
}