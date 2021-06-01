using System.Collections.Generic;
using System.Linq;

namespace SLR.Types
{
    public class TableRule
    {
        public string Key { get; }
        public Dictionary<string, RuleItems> Values { get; }
        
        public TableRule(string key, IEnumerable<string> keys)
        {
            Key = key;
            Values = keys.ToDictionary(x => x, _ => new RuleItems());
        }

        public void QuickAdd(RuleItem ruleItem)
        {
            if (Values.ContainsKey(ruleItem.Value))
                Values[ruleItem.Value].Add(ruleItem);
        }

        public override string ToString()
        {
            return $"{Key} | {string.Join(" ", Values.Select(x => x.Value))}";
        }
        
    }
}