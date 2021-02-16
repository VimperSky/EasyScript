namespace Lexer
{
    public class Token
    {
        public readonly int Line;
        public readonly int Position;
        public readonly TokenType Type;
        public readonly string Value;


        public Token(TokenType tokenType, string value, int line, int position)
        {
            Type = tokenType;
            Value = value;
            Line = line;
            Position = position;
        }

        public override string ToString()
        {
            return $"{Type}: \"{Value}\", line: {Line}, pos: {Position}";
        }
    }
}