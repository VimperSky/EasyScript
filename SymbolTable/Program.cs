using Lexer.Types;
using SymbolTable;

/*
bool aboba = true;
string kek = "kek";
int mek = 7;
float dek = 5.3;
if (mek == 7) 
{
  int dek = 6;
  float mek = 8;
  if (mek > dek) 
  {
    int aboba = 123;
  }
}
*/
var st = new SymbolTableProcessor();
st.CreateTable();
st.Push(TokenType.Bool, "aboba");
st.Push(TokenType.Str, "kek");
st.Push(TokenType.Int, "mek");
st.Push(TokenType.Float, "dek");
st.Push(TokenType.Int, "dek");

Console.WriteLine(st.Find("mek"));
st.CreateTable();
st.Push(TokenType.Int, "dek");
st.Push(TokenType.Float, "mek");
Console.WriteLine(st.Find("mek"));
Console.WriteLine(st.Find("dek"));
st.CreateTable();
st.Push(TokenType.Int, "aboba");
Console.WriteLine(st.Find("aboba"));