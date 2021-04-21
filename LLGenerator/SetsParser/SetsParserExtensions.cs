using System;
using System.Collections.Generic;
using System.Linq;
using LLGenerator.Entities;

namespace LLGenerator.SetsParser
{
    internal static class SetsParserExtensions
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static char GetNextFreeLetter(HashSet<string> takenLetters)
        {
            var freeLetters = Alphabet.ToList();
            freeLetters.RemoveAll(x => takenLetters.Contains(x.ToString()));
            return freeLetters.First();
        }

        public static List<RuleItem> FindCommon(this Rule a, Rule b)
        {
            var minLen = Math.Min(a.Items.Count, b.Items.Count);
            var common = new List<RuleItem>();
            for (var i = 0; i < minLen; i++)
            {
                if (!a.Items[i].Equals(b.Items[i]))
                    break;

                common.Add(a.Items[i]);
            }

            return common;
        }
    }
}