using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Util;

namespace RegexParser
{
    public class MatchCollection2 : CachedList<Match2>
    {
        internal MatchCollection2(IEnumerable<Match2> matches)
            : base(matches)
        {
        }

        protected override void OnNextOriginal(Match2 match, int originalIndex)
        {
            match.NextMatchFunc = () => IsValidIndex(originalIndex + 1) ? this[originalIndex + 1] : Match2.Empty;
        }

        public int Count { get { return this.Count(); } }

        public override string ToString()
        {
            return string.Format("MatchColl <{0}>", 
                                 this.FirstOrDefault() != null ? string.Join(", ", this.Select(m => m.ToString()).ToArray()) :
                                                                 "empty");
        }
    }
}
