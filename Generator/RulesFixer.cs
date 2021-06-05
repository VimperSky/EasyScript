using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Types;

namespace Generator
{
    public class RulesFixer
    {
        private readonly LettersProvider _lettersProvider;

        public RulesFixer(LettersProvider lettersProvider)
        {
            _lettersProvider = lettersProvider;
        }

        private void InsertRuleAtStart(IList<Rule> rules, bool letterFromEnd = false)
        {
            rules.Insert(0, new Rule
            {
                NonTerminal = _lettersProvider.GetNextFreeLetter(letterFromEnd),
                Items = new List<RuleItem> {new(rules[0].NonTerminal, ElementType.NonTerminal)}
            });
        }
        
        public ImmutableList<Rule> FixRules(ImmutableList<Rule> rules, bool slr = false)
        {
            if (rules[0].Items[^1].Type is not ElementType.End)
            {
                if (rules.Count(x => x.NonTerminal == rules[0].NonTerminal) > 1) 
                    InsertRuleAtStart(rules, true);
                rules[0].Items.Add(new RuleItem(Constants.EndSymbol, ElementType.End));
            }
            
            if (slr && rules[0].Items.Any(x => x.Value == rules[0].NonTerminal)) 
                InsertRuleAtStart(rules, true);

            return rules;
        }
    }
}