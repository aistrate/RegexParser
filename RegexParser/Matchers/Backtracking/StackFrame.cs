using RegexParser.Patterns;
using Utility.ConsLists;

namespace RegexParser.Matchers.Backtracking
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public abstract class StackFrame
    {
        protected StackFrame(StackFrame parent, IConsList<BasePattern> remainingChildren)
        {
            Parent = parent;
            RemainingChildren = remainingChildren;
        }

        public StackFrame Parent { get; private set; }
        public IConsList<BasePattern> RemainingChildren { get; private set; }
    }
}
