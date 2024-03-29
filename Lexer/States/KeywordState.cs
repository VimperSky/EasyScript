﻿namespace Lexer.States;

public class KeywordState : ILexerState
{
    public LexerMachine.LexerMachine Process(LexerMachine.LexerMachine machine)
    {
        // let
        if (machine.IsExpectedValueContinue) return machine.AddChar();

        // letk lek
        if (machine.IsIdentifierPredict) return machine.SetIdentifierState().AddChar();

        // let; let" let/
        if (machine.IsExpectedValueAchieved)
        {
            machine.GenerateKeyWord();

            if (machine.IsServiceStart)
                return machine.SetServiceOrComment();

            if (machine.IsStringSymbol) return machine.SetError();
        }

        // le; le" le/
        return machine.SetIdentifierState().ReProcess();
    }
}