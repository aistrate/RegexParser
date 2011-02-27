using System.Collections.Generic;

namespace RegexParser.Util
{
    /// <summary>
    /// Provides backdoor access to non-public constructors. To be used for unit testing only.
    /// </summary>
    public static class Factory
    {
        public static MatchCollection2 CreateMatchCollection(IEnumerable<Match2> matches)
        {
            return new MatchCollection2(matches);
        }

        public static Match2 CreateMatch(int index, int length, string value)
        {
            return new Match2(index, length, value);
        }
    }
}
