namespace Lexer
{
    public static class Extensions
    {
        public static string ToLower1(this string str)
        {
            return char.ToLower(str[0]) + str[1..];
        }
    }
}