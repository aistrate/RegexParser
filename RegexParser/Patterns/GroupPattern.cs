using System;
using System.Collections.Generic;
using System.Linq;
using Utility.General;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public class GroupPattern : BasePattern, IEquatable<GroupPattern>
    {
        public GroupPattern(IEnumerable<BasePattern> patterns)
            : this(patterns.ToArray())
        {
        }

        public GroupPattern(params BasePattern[] patterns)
            : base(PatternType.Group, patterns.Sum(p => p.MinCharLength))
        {
            Patterns = patterns;
        }

        public static GroupPattern Empty = new GroupPattern();

        public BasePattern[] Patterns { get; private set; }

        public int Length { get { return Patterns.Length; } }

        public override PPElement ToPrettyPrint()
        {
            return new PPGroup(
                            new PPText("Group"),
                            PPGroupWithDelimiters(Patterns.Select(p => p.ToPrettyPrint())));
        }

        bool IEquatable<GroupPattern>.Equals(GroupPattern other)
        {
            return other != null && this.Patterns.SequenceEqual(other.Patterns);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Patterns.Select(p => p.GetHashCode()).ToArray());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<GroupPattern>)this).Equals(obj as GroupPattern);
        }
    }
}
