using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Util
{
    /// <summary>
    /// Provides backdoor access to non-public constructors. To be used for unit testing only.
    /// </summary>
    public static class Factory
    {
        public static MatchCollection2 CreateMatchCollection(Match2 firstMatch)
        {
            return new MatchCollection2(firstMatch);
        }

        public static MatchCollection2 CreateMatchCollection(Func<Match2> getFirstMatch)
        {
            return new MatchCollection2(getFirstMatch);
        }

        public static Match2 CreateMatch(int index, int length, string value)
        {
            return new Match2(index, length, value);
        }

        public static Match2 CreateMatch(int index, int length, string value, Func<Match2> nextMatchFunc)
        {
            return new Match2(index, length, value, nextMatchFunc);
        }
    }
}
