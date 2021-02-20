using System.Linq;
using Lexer.States;
using static Lexer.Constants;

namespace Lexer.LexerMachine
{
    public partial class LexerMachine
    {
        private ILexerState _lexerState;

        public LexerMachine SetIdleState()
        {
            _lexerState = new IdleState();
            return this;
        }

        public LexerMachine SetKeywordState()
        {
            _expectedValues = KeyWords.Where(x => x.StartsWith(_lastChar)).ToArray();
            _lexerState = new KeywordState();
            return this;
        }

        public LexerMachine SetIdentifierState()
        {
            _lexerState = new IdentifierState();
            return this;
        }

        public LexerMachine SetStringState()
        {
            _lexerState = new StringState();
            return this;
        }

        public LexerMachine SetNumberState()
        {
            _lexerState = new NumberState();
            return this;
        }

        public LexerMachine SetCommentState()
        {
            _lexerState = new CommentState();
            return this;
        }
        
        
        public LexerMachine SetServiceState()
        {
            _expectedValues = ServiceSymbols.Keys.Where(x => x.StartsWith(_lastChar) && x != Comment).ToArray();
            _lexerState = IsCommentStart ? new CommentState() : new ServiceState();
            return this;
        }
    }
}