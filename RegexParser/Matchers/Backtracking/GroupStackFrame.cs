using RegexParser.Patterns;
using Utility.ConsLists;

namespace RegexParser.Matchers.Backtracking
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class GroupStackFrame : StackFrame
    {
        public GroupStackFrame(StackFrame parent, params BasePattern[] remainingChildren)
            : this(parent, new ArrayConsList<BasePattern>(remainingChildren))
        {
        }

        public GroupStackFrame(StackFrame parent, IConsList<BasePattern> remainingChildren)
            : base(parent, remainingChildren)
        {
        }

        public GroupStackFrame MoveToNextChild()
        {
            return new GroupStackFrame(Parent, RemainingChildren.Tail);
        }
    }
}
