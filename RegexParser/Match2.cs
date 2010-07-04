using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Util;

namespace RegexParser
{
    public class Match2 : IEquatable<Match2>
    {
        private Match2()
        {
            Success = false;

            Value = "";
        }

        internal Match2(int index, int length, string value, Func<Match2> nextMatch)
        {
            Success = true;

            Index = index;
            Length = length;
            Value = value;

            if (nextMatch != null)
                this.nextMatch = nextMatch;
        }

        public bool Success { get; private set; }

        public int Index { get; private set; }
        public int Length { get; private set; }
        public string Value { get; private set; }

        public Match2 NextMatch()
        {
            return nextMatch();
        }
        private Func<Match2> nextMatch = () => Match2.Empty;

        public static Match2 Empty = new Match2();

        public override string ToString()
        {
            if (Success)
                return string.Format("Match {{Index={0}, Length={1}, Value=\"{2}\"}}", Index, Length, Value);
            else
                return string.Format("Match {{Success={0}}}", Success);
        }

        bool IEquatable<Match2>.Equals(Match2 other)
        {
            return other != null && this.Success == other.Success &&
                   this.Index == other.Index && this.Length == other.Length && this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Success.GetHashCode(), Index.GetHashCode(), Length.GetHashCode(), Value.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Match2>)this).Equals(obj as Match2);
        }
    }
}
