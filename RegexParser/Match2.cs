using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser
{
    public class Match2
    {
        private Match2()
        {
            Success = false;

            Index = -1;
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

        public override string ToString()
        {
            if (Success)
                return string.Format("Match {{Index={0}, Length={1}, Value=\"{2}\"}}", Index, Length, Value);
            else
                return string.Format("Match {{Success={0}}}", Success);
        }

        public Match2 NextMatch()
        {
            return nextMatch();
        }
        private Func<Match2> nextMatch = () => Match2.Empty;

        public static Match2 Empty { get { return new Match2(); } }
    }
}
