using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public class LettersProvider
    {
        private static readonly Lazy<LettersProvider> Lazy = new(() => new LettersProvider());

        public static LettersProvider Instance => Lazy.Value;

        // Without R
        private const string Alphabet = "ABCDEFGHIJKLMNOPQSTUVWXYZ";

        private readonly List<char> _freeLetters;
        public LettersProvider()
        {
            _freeLetters = Alphabet.ToList();
        }

        public char GetNextFreeLetter(bool fromEnd = false)
        {
            var letter = fromEnd ? _freeLetters.Last() : _freeLetters.First();

            _freeLetters.Remove(letter);

            return letter;
        }
    }
}