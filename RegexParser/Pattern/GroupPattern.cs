using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using RegexParser.Util;

namespace RegexParser.Pattern
{
    public class GroupPattern : BasePattern, IEnumerable<BasePattern>, IEquatable<GroupPattern>
    {
        public GroupPattern(IEnumerable<BasePattern> patterns)
        {
            Patterns = patterns.ToArray();
        }

        public BasePattern[] Patterns { get; private set; }

        public int Length { get { return Patterns.Length; } }

        public override string ToString()
        {
            return string.Format("Group <{0}>", string.Join(", ", Patterns.Select(p => p.ToString()).ToArray()));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Patterns.GetEnumerator();
        }

        public IEnumerator<BasePattern> GetEnumerator()
        {
            return Patterns.Cast<BasePattern>().GetEnumerator();
        }

        bool IEquatable<GroupPattern>.Equals(GroupPattern other)
        {
            return other != null && this.SequenceEqual(other);
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
