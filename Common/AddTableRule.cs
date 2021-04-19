namespace Common
{
    public class AddTableRule: TableRule
    {
        public bool IsShift { get; init; }
        public bool MoveToStack { get; init; }
        public bool IsEnd { get; init; }

        public override string ToString()
        {
            return base.ToString() + $"    {IsShift}    {MoveToStack}   {IsEnd}";
        }
    }
}