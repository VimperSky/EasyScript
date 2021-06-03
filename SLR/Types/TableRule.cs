using System;
using System.Collections.Generic;
using System.Linq;

namespace SLR.Types
{
    public class TableRule
    {
        public TableRule(string key, IEnumerable<string> keys)
        {
            Key = key;
            Values = keys.ToDictionary(x => x, _ => new RuleItems());
        }

        public string Key { get; }
        public Dictionary<string, RuleItems> Values { get; }

        public void QuickAdd(RuleItem ruleItem)
        {
            if (ruleItem.Type == ElementType.Empty)
                throw new ArgumentException("TableItem cannot be empty!");
            
            if (!Values.ContainsKey(ruleItem.Value)) 
                throw new ArgumentException("Wrong ruleItem index! " + ruleItem.Value);
            
            if (Values[ruleItem.Value].Any(x => x.Type == ElementType.Fold))
                throw new ArgumentException($"Trying to add item to key which was folded: {ruleItem.Value}");

            if (!Values[ruleItem.Value].Contains(ruleItem))
                Values[ruleItem.Value].Add(ruleItem);
        }

        public void QuickFold(string key, int index)
        {
            if (!Values.ContainsKey(key)) 
                throw new ArgumentException("Wrong ruleItem index! " + key);

            if (Values[key].Any(x => x.Type == ElementType.Fold))
                throw new ArgumentException($"Trying to fold by key which already folded: {key}");

            Values[key].Add(new RuleItem($"R{index}", ElementType.Fold));
        }

        public override string ToString()
        {
            return $"{Key} | {string.Join(" ", Values.Select(x => x.Value))}";
        }
    }
}