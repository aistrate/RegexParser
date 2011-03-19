using RegexParser.Patterns;
using Utility.ConsLists;

namespace RegexParser.Matchers.Backtracking
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class QuantifierStackFrame : StackFrame
    {
        public QuantifierStackFrame(StackFrame parent, QuantifierPattern quant)
            : this(parent, new RepeaterConsList<BasePattern>(quant.ChildPattern, quant.MaxOccurrences), quant.IsGreedy, -1)
        {
            if (quant.MinOccurrences != 0)
                quant.AssertCanonicalForm();
        }

        private QuantifierStackFrame(StackFrame parent, IConsList<BasePattern> remainingChildren,
                                     bool isGreedy, int lastPosition)
            : base(parent, remainingChildren)
        {
            IsGreedy = isGreedy;
            LastPosition = lastPosition;
        }

        public bool IsGreedy { get; private set; }
        public int LastPosition { get; private set; }


        public bool IsPositionChanged(int lastPosition)
        {
            return lastPosition > LastPosition;
        }

        public StackFrame FirstAlternative(int lastPosition)
        {
            return IsGreedy ? nonEmptyBranch(lastPosition) : emptyBranch();
        }

        public StackFrame SecondAlternative(int lastPosition)
        {
            return IsGreedy ? emptyBranch() : nonEmptyBranch(lastPosition);
        }

        private StackFrame nonEmptyBranch(int lastPosition)
        {
            return new GroupStackFrame(moveToNextChild(lastPosition), RemainingChildren.Head);
        }

        private StackFrame emptyBranch()
        {
            return Parent;
        }

        private QuantifierStackFrame moveToNextChild(int lastPosition)
        {
            return new QuantifierStackFrame(Parent, RemainingChildren.Tail, IsGreedy, lastPosition);
        }
    }
}
