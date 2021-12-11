namespace Lexer.States;

public class ServiceState : ILexerState
{
    public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
    {
        // != //
        if (machine.IsExpectedValueContinue) return machine.AddChar();

        // !== or !=; or !=k or !=/ or != ...
        if (machine.IsExpectedValueAchieved) return machine.GenerateServiceSymbol().ReProcess();

        // !k or ! or !;
        return machine.SetError();
    }
}