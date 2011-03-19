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
            : this(parent, new RepeaterConsList<BasePattern>(quant.ChildPattern, quant.MaxOccurrences), quant.IsGreedy)
        {
            if (quant.MinOccurrences != 0)
                quant.AssertCanonicalForm();
        }

        private QuantifierStackFrame(StackFrame parent, IConsList<BasePattern> remainingChildren, bool isGreedy)
            : base(parent, remainingChildren)
        {
            IsGreedy = isGreedy;
        }

        public bool IsGreedy { get; private set; }

        public override StackFrame MoveToNextChild()
        {
            return new QuantifierStackFrame(Parent, RemainingChildren.Tail, IsGreedy);
        }
    }
}
