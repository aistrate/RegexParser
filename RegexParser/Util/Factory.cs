using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Util
{
    /// <summary>
    /// Gives access to non-public constructors. Used only for testing (with NUnit).
    /// </summary>
    public static class Factory
    {
        public static Match2 CreateMatch(int index, int length, string value, Func<Match2> nextMatch)
        {
            return new Match2(index, length, value, nextMatch);
        }
    }
}
