using System.Collections.Generic;

namespace LL1TGenerator.Entities
{
    public class TableRule
    {
        public int Id { get; init; }
        public string NonTerminal { get; init; }
        public HashSet<string> DirSet { get; init; }
        public int? GoTo { get; set; }
        public bool IsError { get; init; }
        
        // Дополнительные поля
        public bool IsShift { get; init; }
        public bool MoveToStack { get; init; }
        public bool IsEnd { get; init; }

        public override string ToString()
        {
            return $"Id: {Id} NonTerm: {NonTerminal} Dirs: {string.Join(", ", DirSet)} Goto: {GoTo} Err: {IsError}" +
                   $" Shift: {IsShift} Stack: {MoveToStack} End: {IsEnd}" ;
        }
    }
}