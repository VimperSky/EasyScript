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

        public LexerMachine SetIntState()
        {
            _lexerState = new IntState();
            return this;
        }

        public LexerMachine SetFloatState()
        {
            _lexerState = new FloatState();
            return this;
        }

        private LexerMachine SetCommentState()
        {
            _lexerState = new SingleComment();
            return this;
        }

        public LexerMachine SetServiceState()
        {
            _lexerState = new ServiceState();
            return this;
        }

        private LexerMachine SerErrorState()
        {
            _lexerState = new ErrorState();
            return this;
        }

        /// <summary>
        ///     Sets error state and reprocesses it
        /// </summary>
        /// <returns></returns>
        public LexerMachine SetError()
        {
            return SerErrorState().ReProcess();
        }

        /// <summary>
        ///     Sets comment state without adding last char or service state with last char
        /// </summary>
        /// <returns></returns>
        public LexerMachine SetServiceOrComment()
        {
            _expectedValues = ServiceSymbols.Keys.Where(x => x.StartsWith(_lastChar) && x != Constants.SingleComment)
                .ToArray();
            return AddChar().IsCommentSymbol ? SetCommentState() : SetServiceState();
        }

        public LexerMachine SetMultiLineCommentState()
        {
            _lexerState = new MultiComment();
            return this;
        }
    }
}