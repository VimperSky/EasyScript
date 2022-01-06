using System;
using System.Collections.Generic;
using System.Linq;
using Generator.RulesProcessing;
using Generator.Types;
using SLR.Types;

namespace SLR;

public class Analyzer
{
    private readonly IRulesProcessor _rulesProcessor;
    private readonly ActionManager _actionManager;

    public Analyzer(IRulesProcessor rulesProcessor)
    {
        _rulesProcessor = rulesProcessor;
        
        _actionManager = new ActionManager();
    }
    
    public void Analyze(IEnumerable<string> inputTokens, List<TableRule> table, List<Rule> rules)
    {
        var leftStack = new Stack<string>();
        var rightStack = new Stack<string>();
        var inputStack = new Stack<string>();
        foreach (var inp in inputTokens.Reverse())
            inputStack.Push(inp);

        rightStack.Push(table.First().Key);
        while (true)
        {
            try
            {
                var token = inputStack.Count > 0 ? inputStack.Peek() : _rulesProcessor.EmptyToken;
                
                var (_, rowItems) = table.First(x => x.Key == rightStack.Peek()).Values
                    .SingleOrDefault(x => x.Key == token);

                if (rowItems == null)
                    throw new Exception("Items are empty");

                if (rowItems.Count == 0)
                    continue;

                var firstRowItem = rowItems.First();
                if (firstRowItem.Value == "OK")
                {
                    Console.WriteLine("Analyzer correct!");
                    return;
                }

                if (firstRowItem.Type is ElementType.Collapse)
                {
                    // Правило, по которому сворачиваемся
                    var rule = rules[int.Parse(firstRowItem.Value.Substring(1, firstRowItem.Value.Length - 1)) - 1];
                    

                    if (rule.Items[0].Type is not ElementType.Empty)
                    {
                        for (var i = 0; i < rule.Items.Count && rule.Items[i].Type is not ElementType.End; i++)
                        {
                            leftStack.Pop();
                            rightStack.Pop();
                        }
                    }

                    if (rightStack.Count == 1 && leftStack.Count == 0 && inputStack.Count == 0)
                    {
                        Console.WriteLine("Analyzer correct!");
                        return;
                    }

                    inputStack.Push(rule.NonTerminal);
                }
                else
                {
                    inputStack.Pop();
                    rightStack.Push(rowItems.ToString());
                    leftStack.Push(token);
                }
                
                Console.WriteLine($"Left [{string.Join(", ", leftStack.ToArray())}]" +
                                  $" Input [{string.Join(" ", inputStack.ToArray())}]" +
                                  $" Right [{string.Join(", ", rightStack.ToArray())}]");
            }
            catch (Exception e)
            {
                throw new ArgumentException("[Syntax Analyzer Error] " + e + "\r\n*** Analyzer State ***" +
                                            $"\r\nLeft [{string.Join(", ", leftStack.ToArray())}]" +
                                            $"\r\nInput [{string.Join(" ", inputStack.ToArray())}]" +
                                            $"\r\nRight [{string.Join(", ", rightStack.ToArray())}]");
            }
        }
    }
}