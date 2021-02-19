using Lexer.States;

namespace Lexer.Tests
{
    public class Extension
    {
        public static void ProcessString(string line, LexerMachine machine)
        {
            foreach (var t in line)
            {
                machine.ProcessChar(t, 0, 0);
            }

            machine.ProcessChar('\n', 0, 0);
        }
    }
}