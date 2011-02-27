using RegexParser.Patterns;
using Utility.ConsLists;

namespace RegexParser.Matchers
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class StackFrame
    {
        public StackFrame(StackFrame parent, params BasePattern[] remainingChildren)
            : this(parent, new ArrayConsList<BasePattern>(remainingChildren))
        {
        }

        public StackFrame(StackFrame parent, IConsList<BasePattern> remainingChildren)
        {
            Parent = parent;
            RemainingChildren = remainingChildren;
        }

        public StackFrame Parent { get; private set; }
        public IConsList<BasePattern> RemainingChildren { get; private set; }
    }
}
