using System;
using System.Collections.Generic;
using Utility.General;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public enum AnchorType
    {
        StartOfStringOrLine = 1,
        EndOfStringOrLine,
        StartOfStringOnly,
        EndOfStringOnly,
        EndOfStringOrBeforeEndingNewline,
        ContiguousMatch = 32,
        WordBoundary,
        NonWordBoundary,
    }

    public enum ExplicitAnchorType
    {
        StartOfString = 16,
        StartOfLine,
        EndOfString,
        EndOfLine,
        EndOfStringOrBeforeEndingNewline,
        ContiguousMatch = 32,
        WordBoundary,
        NonWordBoundary,
    }

    public class AnchorPattern : BasePattern, IEquatable<AnchorPattern>
    {
        public AnchorPattern(AnchorType anchorType)
            : base(PatternType.Anchor, 0)
        {
            AnchorType = anchorType;

            if (anchorType < AnchorType.ContiguousMatch)
                ExplicitAnchorType = defaultExplicitAnchorType[anchorType];
            else
                ExplicitAnchorType = (ExplicitAnchorType)anchorType;
        }

        public AnchorPattern(AnchorType anchorType, ExplicitAnchorType explicitAnchorType)
            : base(PatternType.Anchor, 0)
        {
            AnchorType = anchorType;
            ExplicitAnchorType = explicitAnchorType;
        }

        public AnchorType AnchorType { get; private set; }
        public ExplicitAnchorType ExplicitAnchorType { get; private set; }

        private static Dictionary<AnchorType, ExplicitAnchorType> defaultExplicitAnchorType =
            new Dictionary<AnchorType, ExplicitAnchorType>()
            {
                { AnchorType.StartOfStringOrLine, ExplicitAnchorType.StartOfString },
                { AnchorType.EndOfStringOrLine, ExplicitAnchorType.EndOfString },
                { AnchorType.StartOfStringOnly, ExplicitAnchorType.StartOfString },
                { AnchorType.EndOfStringOnly, ExplicitAnchorType.EndOfString },
                { AnchorType.EndOfStringOrBeforeEndingNewline, ExplicitAnchorType.EndOfStringOrBeforeEndingNewline },
            };

        public override PPElement ToPrettyPrint()
        {
            return new PPText(Type.ToString(),
                              string.Format("Anchor ({0}{1})",
                                            AnchorType.ToString(),
                                            ExplicitAnchorType.ToString() != AnchorType.ToString() ?
                                                ", " + ExplicitAnchorType.ToString() :
                                                ""));
        }

        bool IEquatable<AnchorPattern>.Equals(AnchorPattern other)
        {
            return other != null &&
                   this.AnchorType == other.AnchorType &&
                   this.ExplicitAnchorType == other.ExplicitAnchorType;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(AnchorType.GetHashCode(), ExplicitAnchorType.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<AnchorPattern>)this).Equals(obj as AnchorPattern);
        }
    }
}
