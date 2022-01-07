using System.Collections.Generic;
using System.Linq;
using Generator;
using Generator.RulesProcessing;
using Generator.Types;
using SLR.Types;

namespace SLR.Table
{
    public class TableBuilder
    {
        private readonly List<Rule> _rules;
        private readonly List<string> _valueKeys;

        public TableBuilder(List<Rule> rules)
        {
            _rules = rules;
            var valueKeys = new HashSet<string>();
            foreach (var rule in rules)
            {
                valueKeys.Add(rule.NonTerminal);
                foreach (var ruleItem in rule.Items.Where(ruleItem =>
                    ruleItem.Type is not ElementType.Empty))
                    valueKeys.Add(ruleItem.Value);
            }

            _valueKeys = valueKeys.ToList();
        }

        private TableRule CreateTableRule(string key) => new(key, _valueKeys);

        private RuleItem GetItem(int ruleIndex, int itemIndex) => _rules[ruleIndex].Items[itemIndex];

        public List<TableRule> CreateTable()
        {
            // Таблица и список правил на разбор и добавление в таблицу
            var tableRules = new List<TableRule>();
            var pendingItems = new Queue<RuleItems>();

            {
                // Начальный проход с первого нетерминала
                var tableRule = CreateTableRule(_rules[0].NonTerminal);
                tableRule.Values[_rules[0].NonTerminal] = new RuleItems {new("OK", ElementType.Terminal)};
                First(tableRule, _rules[0].NonTerminal);

                UpdatePendingItems(tableRule);
            }

            // Проход по остальным правилам, берем последовательно с очереди
            while (pendingItems.Count > 0)
            {
                var items = pendingItems.Dequeue();
                var key = items.ToString();
                if (key == "OK") continue;
                if (tableRules.Any(x => x.Key == key)) continue;
                var tableRule = CreateTableRule(key);
                foreach (var item in items) Follow(tableRule, item);
                UpdatePendingItems(tableRule);
            }

            return tableRules.ToList();

            void UpdatePendingItems(TableRule tableRule)
            {
                foreach (var item in tableRule.Values
                    .Where(x => x.Value.Count > 0))
                {
                    var value = item.Value;
                    if (tableRules.Any(x => x.Key == value.ToString())) continue;

                    if (value.Any(x => x.Type is ElementType.Collapse)) continue;

                    pendingItems.Enqueue(value);
                }

                tableRules.Add(tableRule);
            }
        }

        private void Follow(TableRule tableRule, RuleItem item)
        {
            // Это последний элемент в правиле
            if (item.ItemIndex >= _rules[item.RuleIndex].Items.Count - 1)
            {
                var nextItems = _rules.FindNextRecursive(_rules[item.RuleIndex].NonTerminal);
                foreach (var nextItem in nextItems)
                {
                    tableRule.QuickCollapse(nextItem, item.RuleIndex + 1);
                    if (nextItem.Type is ElementType.NonTerminal)
                        FirstCollapse(tableRule, nextItem.Value, item.RuleIndex + 1);
                }

                return;
            }

            // Берем следующий
            var next = GetItem(item.RuleIndex, item.ItemIndex + 1);
            if (next.Type == ElementType.End)
            {
                tableRule.QuickCollapse(next, item.RuleIndex + 1);
                return;
            }

            tableRule.QuickAdd(next);
            if (next.Type == ElementType.NonTerminal) First(tableRule, next.Value);
        }

        private void FirstCollapse(TableRule tableRule, string nonTerm, int collapseIndex)
        {
            foreach (var rules in _rules.Where(x => x.NonTerminal == nonTerm))
            {
                var first = rules.Items[0];
                switch (first.Type)
                {
                    case ElementType.Terminal or ElementType.End:
                        tableRule.QuickCollapse(first, collapseIndex);
                        break;
                    case ElementType.NonTerminal when nonTerm != first.Value:
                        FirstCollapse(tableRule, first.Value, collapseIndex);
                        break;
                }
            }
        }

        private void First(TableRule tableRule, string nonTerm)
        {
            foreach (var rules in _rules.Where(x => x.NonTerminal == nonTerm))
            {
                var first = rules.Items[0];
                if (first.Type is ElementType.Terminal or ElementType.NonTerminal or ElementType.End)
                {
                    tableRule.QuickAdd(first);
                    if (first.Type is ElementType.NonTerminal && nonTerm != first.Value) 
                        First(tableRule, first.Value);
                }
            }
        }
    }
}