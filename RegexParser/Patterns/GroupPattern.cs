using System;
using System.Collections.Generic;
using System.Linq;
using Utility.General;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public class GroupPattern : BasePattern, IEquatable<GroupPattern>
    {
        public GroupPattern(bool isCapturing, IEnumerable<BasePattern> patterns)
            : this(isCapturing, patterns.ToArray())
        {
        }

        public GroupPattern(bool isCapturing, params BasePattern[] patterns)
            : base(PatternType.Group, patterns.Sum(p => p.MinCharLength))
        {
            IsCapturing = isCapturing;
            Patterns = patterns;
        }

        public static GroupPattern Empty = new GroupPattern(false);

        public bool IsCapturing { get; private set; }

        public BasePattern[] Patterns { get; private set; }

        public override PPElement ToPrettyPrint()
        {
            return new PPGroup(
                            Type.ToString(),
                            new PPText(string.Format("Group ({0})", IsCapturing ? "Capt" : "NC")),
                            PPGroupWithDelimiters(Patterns.Select(p => p.ToPrettyPrint())));
        }

        bool IEquatable<GroupPattern>.Equals(GroupPattern other)
        {
            return other != null &&
                   this.IsCapturing == other.IsCapturing &&
                   this.Patterns.SequenceEqual(other.Patterns);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Patterns.Select(p => p.GetHashCode())
                                                    .Concat(new int [] { IsCapturing.GetHashCode() })
                                                    .ToArray());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<GroupPattern>)this).Equals(obj as GroupPattern);
        }
    }
}
