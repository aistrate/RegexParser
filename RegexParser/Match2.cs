using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;
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

        internal Match2(int index, int length, string value)
        {
            Success = true;
            Index = index;
            Length = length;
            Value = value;
        }

        public bool Success { get; private set; }

        public int Index { get; private set; }
        public int Length { get; private set; }
        public string Value { get; private set; }

        public MatchCollection2 Parent { get; internal set; }
        public int ParentIndex { get; internal set; }

        public Match2 NextMatch()
        {
            return Parent != null && Parent.IsValidIndex(ParentIndex + 1) ? Parent[ParentIndex + 1] : Match2.Empty;
        }

        public static Match2 Empty = new Match2();

        public override string ToString()
        {
            if (Success)
                return string.Format("Match {{Index={0}, Length={1}, Value={2}}}", Index, Length, Value.Show());
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
