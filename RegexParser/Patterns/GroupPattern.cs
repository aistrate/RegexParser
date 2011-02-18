using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using RegexParser.Util;
using ParserCombinators.Util;

namespace RegexParser.Patterns
{
    public class GroupPattern : BasePattern, IEquatable<GroupPattern>
    {
        public GroupPattern(IEnumerable<BasePattern> patterns)
        {
            Patterns = patterns.ToArray();
        }

        public GroupPattern(params BasePattern[] patterns)
        {
            Patterns = patterns;
        }

        public static GroupPattern Empty = new GroupPattern();

        public BasePattern[] Patterns { get; private set; }

        public int Length { get { return Patterns.Length; } }

        public override string ToString()
        {
            return string.Format("<{0}>", Patterns.Select(p => p.ToString())
                                                  .JoinStrings(", "));
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
