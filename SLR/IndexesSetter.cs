using System.Collections.Generic;
using Generator.Types;

namespace SLR
{
    public static class IndexesSetter
    {
        public static void SetIndexes(List<Rule> rules)
        {
            for (var i = 0; i < rules.Count; i++)
            for (var j = 0; j < rules[i].Items.Count; j++)
                rules[i].Items[j].SetIndex(i, j);
        }
    }
}