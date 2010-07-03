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
            foreach (var match in getAllMatches(getFirstMatch()))
                yield return match;
            
            yield break;
        }

        public int Count { get { return this.Count(); } }
    }
}
