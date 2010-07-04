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
        public static Match2 CreateMatch(int index, int length, string value, Func<Match2> nextMatch)
        {
            return new Match2(index, length, value, nextMatch);
        }
    }
}
