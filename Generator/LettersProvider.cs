using System.Collections.Generic;
using System.Linq;

namespace Generator
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

        public void TakeLetter(char letterToTake)
        {
            if (_freeLetters.Contains(letterToTake))
                _freeLetters.Remove(letterToTake);
        }

        public string GetNextFreeLetter(bool fromEnd = false)
        {
            var letter = fromEnd ? _freeLetters.Last() : _freeLetters.First();

            _freeLetters.Remove(letter);

            return letter.ToString();
        }
    }
}