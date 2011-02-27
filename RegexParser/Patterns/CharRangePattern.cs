using System;
using ParserCombinators.Util;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    public class CharRangePattern : CharPattern, IEquatable<CharRangePattern>
    {
        public CharRangePattern(char from, char to)
        {
            From = from;
            To = to;
        }

        public readonly char From;
        public readonly char To;

        public override bool IsMatch(char c)
        {
            return From <= c && c <= To;
        }

        public override string ToString()
        {
            return string.Format("{{{0}-{1}}}", From.Show(), To.Show());
        }

        bool IEquatable<CharRangePattern>.Equals(CharRangePattern other)
        {
            return other != null && this.From == other.From && this.To == other.To;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(From.GetHashCode(), To.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharRangePattern>)this).Equals(obj as CharRangePattern);
        }
    }
}
