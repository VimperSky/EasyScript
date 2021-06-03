using System.Collections.Immutable;
using SLR.Types;

namespace SLR
{
    public class EmptyRemover
    {
        public ImmutableList<Rule> RemoveEmpty(ImmutableList<Rule> inputRules)
        {
            return inputRules;
        }
    }
}