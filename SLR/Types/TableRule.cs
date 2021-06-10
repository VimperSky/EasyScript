using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Types;

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
            // Если такого ключа не существует впринципе
            if (!Values.ContainsKey(ruleItem.Value))
                throw new ArgumentException("Wrong ruleItem index! " + ruleItem.Value);

            // Если мы пытаемся добавить e
            if (ruleItem.Type == ElementType.Empty)
                throw new ArgumentException("TableItem cannot be empty!");

            // Если мы пытаемся добавить обычное значение в клетку, где уже есть свертка
            if (Values[ruleItem.Value].Any(x => x.Type == ElementType.Collapse))
                throw new ArgumentException($"Trying to add item to key which was collapsed: {ruleItem.Value}");

            if (!Values[ruleItem.Value].Contains(ruleItem))
                Values[ruleItem.Value].Add(ruleItem);
        }

        public void QuickCollapse(string key, Rule rule)
        {
            // Если такого ключа не существует в словаре впринципе
            if (!Values.ContainsKey(key))
                throw new ArgumentException("Wrong ruleItem index! " + key);

            var newVal = rule.NonTerminal + "-" + rule.Items.Count;
            // Если уже существуют ключи
            if (Values[key].Count > 0)
            {
                var first = Values[key].First();
                // Если мы пытаемся добавить тоже самое, что уже добавили, то просто делаем возврат
                if (first.Type == ElementType.Collapse && first.Value == newVal)
                    return;

                // Иначе, если мы пытаемся добавить другую свертку или обычное значение, кидаем ошибку
                throw new Exception("Trying to collapse item which was added with regular value. " +
                                    $"\r\n[Info] key: {key}, new value: {newVal}");
            }


            Values[key].Add(new RuleItem(newVal, ElementType.Collapse));
        }

        public override string ToString()
        {
            return $"{Key} | {string.Join(" ", Values.Select(x => x.Value))}";
        }
    }
}