using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;
using Lexer.Types;
using static Lexer.Constants;

namespace Generator.RulesProcessing
{
    public class LexerRulesProcessor: IRulesProcessor
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
            TokenTypes = ServiceSymbols.Concat(KeyWords).Concat(ParserTypes)
                .ToDictionary(x => x.Key, x => x.Value);
        }


        public RuleItem ParseToken(string token)
        {
            if (token.StartsWith("<") && token.EndsWith(">"))
                return new RuleItem(token, ElementType.NonTerminal);

            if (TokenTypes.ContainsKey(token))
            {
                var tokenType = TokenTypes[token];
                var elementType = tokenType switch
                {
                    TokenType.End => ElementType.End,
                    TokenType.Empty => ElementType.Empty,
                    _ => ElementType.Terminal
                };
                return new RuleItem(tokenType.ToString(), elementType);
            }
            
            throw new ArgumentException($"TokenType is not correct. {token}");
        }

        public ImmutableList<Rule> Process(List<(string NonTerminal, string RightBody)> inputRules)
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

            return lexerRules.ToImmutableList();
        }
    }
}