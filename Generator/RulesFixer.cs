using System.Collections.Generic;
using System.Linq;
using Generator.RulesProcessing;
using Generator.Types;

namespace Generator
{
    public class RulesFixer
    {
        private readonly LettersProvider _lettersProvider;
        private readonly IRulesProcessor _rulesProcessor;

        public RulesFixer(IRulesProcessor rulesProcessor, LettersProvider lettersProvider)
        {
            _rulesProcessor = rulesProcessor;
            _lettersProvider = lettersProvider;
        }

        private void InsertRuleAtStart(List<Rule> rules, bool letterFromEnd = false)
        {
            rules.Insert(0, new Rule
            {
                NonTerminal = _lettersProvider.GetNextFreeLetter(letterFromEnd),
                Items = new List<RuleItem> {new(rules[0].NonTerminal, ElementType.NonTerminal)}
            });
        }

        public void FixRules(List<Rule> rules, bool slr = false)
        {
            if (rules[0].Items[^1].Type is not ElementType.End)
            {
                if (rules.Count(x => x.NonTerminal == rules[0].NonTerminal) > 1)
                {
                    InsertRuleAtStart(rules, true);
                }

                rules[0].Items.Add(_rulesProcessor.ParseToken(Constants.EndSymbol));
            }

            if (slr && rules[0].Items.Any(x => x.Value == rules[0].NonTerminal))
            {
                InsertRuleAtStart(rules, true);
            }

            foreach (var letter in rules.Select(x => x.NonTerminal).Where(x => x.Length == 1))
                _lettersProvider.TakeLetter(letter[0]);
        }
    }
}