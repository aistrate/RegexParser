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

        public QuantifierStackFrame MoveToNextChild(int lastPosition)
        {
            return new QuantifierStackFrame(Parent, RemainingChildren.Tail, IsGreedy, lastPosition);
        }
    }
}
