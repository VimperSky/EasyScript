using System;
using System.Collections.Generic;
using System.Linq;
using Generator.RulesProcessing;
using Generator.Types;
using Lexer.Types;
using SLR.Types;

namespace SLR.Analyzer;

public class LexerAnalyzer
{
    private readonly LexerRulesProcessor _rulesProcessor;
    private readonly ActionManager _actionManager;

    public LexerAnalyzer(LexerRulesProcessor rulesProcessor)
    {
        _rulesProcessor = rulesProcessor;
        _actionManager = new ActionManager();
    }
    
    public void Analyze(IEnumerable<Token> inputTokens, List<TableRule> table, List<Rule> rules)
    {
        // Левый стек используется для хранения типов токенов
        // Правый стек используется для хранения табличных значений
        var inputStack = new Stack<Token>();

        var leftStack = new Stack<TokenType>();
        var rightStack = new Stack<string>();
        foreach (var inp in inputTokens.Reverse())
            inputStack.Push(inp);

        rightStack.Push(table.First().Key);

        while (true)
        {
            try
            {
                var token = inputStack.Peek();
                var tableValue = rightStack.Peek();

                // Достаем элемент из правого стека, находим строчку, где этот элемент находится слева таблицы. 
                // Находим в строчке клетку, где этот элемент равен входному токену
                var (_, cell) = table.Single(x => x.Key == tableValue).Values
                    .SingleOrDefault(x => x.Key == token.Type.ToString());

                if (cell == null || cell.Count == 0)
                    throw new AnalyzerInnerException($"In input {token.ToString()} follows the {tableValue}, it is not allowed by rules.");
                
                // В клетке может находится 0 и более элементов. Пример двух элементов: if11, if12.
                var firstItem = cell.First();
                if (firstItem.Value == "OK")
                {
                    Console.WriteLine("Analyzer correct by OK!");
                    return;
                }

                if (firstItem.Type is ElementType.Collapse)
                {
                    // Ищем правило, по которому происходит свертка
                    var rule = rules[int.Parse(firstItem.Value.Substring(1, firstItem.Value.Length - 1)) - 1];

                    // Мы удаляем из левого и правого стека столько элементов, сколько находится в этом правиле, за исключением End символа.
                    for (var i = 0; i < rule.Items.Count && rule.Items[i].Type is not ElementType.End; i++)
                    {
                        leftStack.Pop();
                        rightStack.Pop();
                    }

                    // Фиг пойми зачем это условие, вроде без него работает.
                    // if (rightStack.Count == 1 && leftStack.Count == 0 && inputStack.Count == 0)
                    // {
                    //     Console.WriteLine("Analyzer correct by unknown condition!");
                    //     return;
                    // }

                    inputStack.Push( new Token(_rulesProcessor.ParseTokenType(rule.NonTerminal), );
                    inputStack.Push(rule.NonTerminal);
                }
                else
                {
                    inputStack.Pop();
                    rightStack.Push(cell.ToString());
                    leftStack.Push(token.Type.ToString());
                }

                // Console.WriteLine($"Left [{string.Join(", ", leftStack.ToArray())}]" +
                //                   $" Input [{string.Join(" ", inputStack.ToArray())}]" +
                //                   $" Right [{string.Join(", ", rightStack.ToArray())}]");
            }
            catch (AnalyzerInnerException e)
            {
                throw new AnalyzerException(e.Text, inputStack, leftStack, rightStack);
            }
            catch (Exception e)
            {
                throw new AnalyzerException($"Unhandled exception: {e}", inputStack, leftStack, rightStack);
            }
        }
    }

    private class AnalyzerInnerException: Exception
    {
        public string Text { get; }

        public AnalyzerInnerException(string text)
        {
            Text = text;
        }
    }
}