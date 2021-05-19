using System;
using System.Collections.Generic;
using System.Linq;
using Lexer.Types;
using static Lexer.Constants;

namespace Lexer.RulesParser
{
    public class LexerRulesParser
    {
        private readonly Dictionary<string, TokenType> _tokenTypes;

        private readonly Dictionary<string, TokenType> _parserTypes = new()
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

        public LexerRulesParser()
        {
            _tokenTypes = ServiceSymbols.Concat(KeyWords).Concat(_parserTypes)
                .ToDictionary(x => x.Key, x => x.Value);
        }
        public List<LexerRule> Parse()
        {
            var rules = CsvParser.Parse();
            var lexerRules = new List<LexerRule>();
            foreach (var (nonTerminal, rightBody) in 
                rules.Where(x => !string.IsNullOrWhiteSpace(x.NonTerminal)))
            {
                var tempTokens = new List<LexerToken>();
                foreach (var item in rightBody.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                {
                    if (item == "|")
                    {
                        lexerRules.Add(new LexerRule {NonTerminal = nonTerminal, Tokens = tempTokens});
                        tempTokens = new List<LexerToken>();
                        continue;
                    }

                    if (item.StartsWith("<") && item.EndsWith(">"))
                    {
                        tempTokens.Add(new LexerToken {NonTerminal = item});
                    }
                    else if (_tokenTypes.ContainsKey(item))
                    {
                        tempTokens.Add(new LexerToken {TokenType = _tokenTypes[item]});
                    }
                    else
                    {
                        throw new ArgumentException($"TokenType is not correct. {item}");
                    }
                }
                lexerRules.Add(new LexerRule {NonTerminal = nonTerminal, Tokens = tempTokens});
            }

            return lexerRules;
        }
    }
}