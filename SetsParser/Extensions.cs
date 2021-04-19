using System.Collections.Generic;
using System.Linq;
using Common;

namespace SetsParser
{
    internal static class Extensions
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static char GetNextFreeLetter(HashSet<string> takenLetters)
        {
            var freeLetters = Alphabet.ToList();
            freeLetters.RemoveAll(x => takenLetters.Contains(x.ToString()));
            return freeLetters.First();
        }

        public static List<RuleItem> FindCommon(this IList<Rule> rules)
        {
            var minLen = rules.Min(x => x.Items.Count);
            var common = new List<RuleItem>();
            for (var l = 0; l < minLen; l++)
            {
                var allEqu = true;
                foreach (var r1 in rules)
                foreach (var r2 in rules)
                    if (!r1.Items[l].Equals(r2.Items[l]))
                        allEqu = false;
                if (allEqu)
                    common.Add(rules[0].Items[l]);
            }

            return common;
        }
    }
}