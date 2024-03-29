﻿using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Types;
using Lexer.LexerMachine;
using Lexer.Types;

namespace Generator.RulesProcessing
{
    public class LexerRulesProcessor : IRulesProcessor
    {
        private static readonly Dictionary<string, TokenType> TokenTypes;

        private static readonly Dictionary<string, TokenType> ParserTypes = new()
        {
            {"$", TokenType.End},
            {"e", TokenType.Empty},
            {"id", TokenType.Identifier},
            {"!bool", TokenType.AnyBool},
            {"!float", TokenType.AnyFloat},
            {"!str", TokenType.AnyString},
            {"!int", TokenType.AnyInt},
            {"!|", TokenType.Or}
        };

        static LexerRulesProcessor()
        {
            TokenTypes = LexerMachine.ServiceSymbols.Concat(LexerMachine.KeyWords).Concat(ParserTypes)
                .ToDictionary(x => x.Key, x => x.Value);
        }


        public string EndToken { get; } = TokenType.Empty.ToString();

        public RuleItem ParseToken(string token)
        {
            var splitToken = token.Split("#");
            var tokenValue = splitToken[0];
            var action = splitToken.Length > 1 ? splitToken[1] : null;

            if (tokenValue.StartsWith("<") && tokenValue.EndsWith(">"))
                return new RuleItem(tokenValue, ElementType.NonTerminal, action);

            if (TokenTypes.ContainsKey(tokenValue))
            {
                var tokenType = TokenTypes[tokenValue];
                var elementType = tokenType switch
                {
                    TokenType.End => ElementType.End,
                    TokenType.Empty => ElementType.Empty,
                    _ => ElementType.Terminal
                };
                return new RuleItem(tokenType.ToString(), elementType, action);
            }

            throw new ArgumentException($"TokenType is not correct. {tokenValue}");
        }

        public List<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules)
        {
            var lexerRules = new List<Rule>();
            foreach (var (nonTerminal, rightBody) in inputRules
                .Where(x => !string.IsNullOrWhiteSpace(x.NonTerminal)))
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

                    var token = ParseToken(item);
                    tempTokens.Add(token);
                }

                lexerRules.Add(new Rule {NonTerminal = nonTerminal, Items = tempTokens});
            }

            return lexerRules;
        }
    }
}