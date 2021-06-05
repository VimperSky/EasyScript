using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator;
using Generator.Types;

namespace SLR
{
    public class RulesFixer
    {
        private readonly LettersProvider _lettersProvider;
        public RulesFixer()
        {
            _lettersProvider = new LettersProvider();
        }
        
        private void InsertRuleAtStart(IList<Rule> rules, bool letterFromEnd = false)
        {
            rules.Insert(0, new Rule
            {
                NonTerminal = _lettersProvider.GetNextFreeLetter(letterFromEnd),
                Items = new List<RuleItem> {new(rules[0].NonTerminal, ElementType.NonTerminal)}
            });
        }

        
        public ImmutableList<Rule> FixRules(ImmutableList<Rule> inputRules)
        {
            var rules = inputRules.ToList();
            
            if (rules[0].Items[^1].Value != Constants.EndSymbol)
            {
                if (rules.Count(x => x.NonTerminal == rules[0].NonTerminal) > 1) InsertRuleAtStart(rules, true);
                rules[0].Items.Add(new RuleItem(Constants.EndSymbol, ElementType.End));
            }

            if (rules[0].Items.Any(x => x.Value == rules[0].NonTerminal)) InsertRuleAtStart(rules, true);
            
            for (var i = 0; i < rules.Count; i++)
            for (var j = 0; j < rules[i].Items.Count; j++)
            {
                rules[i].Items[j].SetIndex(i, j);
            }

            return rules.ToImmutableList();
        }
    }
}