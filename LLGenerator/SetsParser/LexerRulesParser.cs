using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Lexer.Types;
using LLGenerator.Entities;
using static Lexer.Constants;

namespace LLGenerator.SetsParser
{
    public static class LexerRulesParser
    {
        private static readonly Dictionary<string, TokenType> TokenTypes;

        private static readonly Dictionary<string, TokenType> ParserTypes = new()
        {
            {"end", TokenType.End},
            {"e", TokenType.Empty},
            {"id", TokenType.Identifier},
            {"!bool", TokenType.AnyBool},
            {"!float", TokenType.AnyFloat},
            {"!str", TokenType.AnyString},
            {"!int", TokenType.AnyInt},
            {"!|", TokenType.Or},
        };

        static LexerRulesParser()
        {
            TokenTypes = ServiceSymbols.Concat(KeyWords).Concat(ParserTypes)
                .ToDictionary(x => x.Key, x => x.Value);
        }
        public static ImmutableList<Rule> Parse(IEnumerable<(string NonTerminal, string RightBody)> rules)
        {
            var lexerRules = new List<Rule>();
            foreach (var (nonTerminal, rightBody) in 
                rules.Where(x => !string.IsNullOrWhiteSpace(x.NonTerminal)))
            {
                var tempTokens = new List<RuleItem>();
                foreach (var item in rightBody.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                {
                    if (item == "|")
                    {
                        lexerRules.Add(new Rule {NonTerminal = nonTerminal, Items = tempTokens});
                        tempTokens = new List<RuleItem>();
                        continue;
                    }

                    if (item.StartsWith("<") && item.EndsWith(">"))
                    {
                        tempTokens.Add(new RuleItem(item));
                    }
                    else if (TokenTypes.ContainsKey(item))
                    {
                        tempTokens.Add(new RuleItem(TokenTypes[item]));
                    }
                    else
                    {
                        throw new ArgumentException($"TokenType is not correct. {item}");
                    }
                }
                lexerRules.Add(new Rule {NonTerminal = nonTerminal, Items = tempTokens});
            }

            return lexerRules.ToImmutableList();
        }
    }
}