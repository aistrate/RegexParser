using System;

namespace RegexParser.Patterns
{
    public enum AnchorType
    {
        StartOfStringOrLine,
        EndOfStringOrLine,
        StartOfStringOnly,
        EndOfStringOrBeforeEndingNewline,
        EndOfStringOnly,
        ContiguousMatch,
        WordBoundary,
        NonWordBoundary,
    }

    public class AnchorPattern : BasePattern, IEquatable<AnchorPattern>
    {
        public AnchorPattern(AnchorType anchorType)
            : base(PatternType.Anchor, 0)
        {
            AnchorType = anchorType;
        }

        public AnchorType AnchorType { get; private set; }

        public override string ToString()
        {
            return string.Format("Anchor({0})", AnchorType.ToString());
        }

        bool IEquatable<AnchorPattern>.Equals(AnchorPattern other)
        {
            return other != null && this.AnchorType == other.AnchorType;
        }

        public override int GetHashCode()
        {
            return AnchorType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<AnchorPattern>)this).Equals(obj as AnchorPattern);
        }
    }
}
