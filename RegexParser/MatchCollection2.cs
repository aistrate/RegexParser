using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser
{
    public class MatchCollection2 // : IEnumerable<Match2>
    {
        internal MatchCollection2(Match2 firstMatch)
        {
            matches.Add(firstMatch);
        }

        private List<Match2> matches = new List<Match2>();
    }
}
