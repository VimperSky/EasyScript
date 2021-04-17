namespace SetsParser
{
    public class RuleItem
    {
        public readonly string Value;
        public readonly bool IsTerminal;

        public RuleItem(string value, bool isTerminal)
        {
            Value = value;
            IsTerminal = isTerminal;
        }
    }
    
}