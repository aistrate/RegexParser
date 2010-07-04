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


        #region Other constructors (for unit testing)

        internal MatchCollection2(Match2 firstMatch)
            : base(getAllMatches(firstMatch))
        {
        }

        internal MatchCollection2(Func<Match2> getFirstMatch)
            : base(getAllMatches(getFirstMatch))
        {
        }

        private static IEnumerable<Match2> getAllMatches(Match2 firstMatch)
        {
            Match2 current = firstMatch;

            while (current != null && current.Success)
            {
                yield return current;
                current = current.NextMatch();
            }

            yield break;
        }

        private static IEnumerable<Match2> getAllMatches(Func<Match2> getFirstMatch)
        {
            if (getFirstMatch != null)
                foreach (var match in getAllMatches(getFirstMatch()))
                    yield return match;
            
            yield break;
        }

        #endregion


        public int Count { get { return this.Count(); } }

        public override string ToString()
        {
            return string.Format("MatchColl <{0}>", 
                                 this.FirstOrDefault() != null ? string.Join(", ", this.Select(m => m.ToString()).ToArray()) :
                                                                 "empty");
        }
    }
}
