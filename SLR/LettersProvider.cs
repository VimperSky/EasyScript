using System.Collections.Generic;
using System.Linq;

namespace SLR
{
    public class LettersProvider
    {
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