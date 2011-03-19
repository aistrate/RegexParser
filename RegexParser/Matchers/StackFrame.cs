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

        public virtual StackFrame MoveToNextChild()
        {
            return new StackFrame(Parent, RemainingChildren.Tail);
        }
    }


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
