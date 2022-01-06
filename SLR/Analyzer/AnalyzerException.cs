using System;
using System.Collections.Generic;

namespace SLR.Analyzer;

public class AnalyzerException: Exception
{
    private string Text { get; }

    private string Trace { get; }

    public AnalyzerException(string text, Stack<string> inputStack, Stack<string> leftStack, Stack<string> rightStack)
    {
        Text = text;

        Trace =
            $"\r\nInput [{string.Join(" ", inputStack.ToArray())}]" +
            $"\r\nLeft [{string.Join(", ", leftStack.ToArray())}]" +
            $"\r\nRight [{string.Join(", ", rightStack.ToArray())}]";
    }

    public override string ToString()
    {
        return $"=== Analyzer Exception ===\r\nMessage: {Text}\r\n\r\n== Trace == {Trace}\r\n=== End of Analyzer Exception ===";
    }
}