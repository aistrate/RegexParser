using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser
{
    public class Regex2
    {
        public Regex2(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; private set; }

        public override string ToString() { return Pattern; }

        public Match2 Match(string text)
        {
            //System.Text.RegularExpressions.Regex.
            
            return Match2.Empty;
        }
    }
}
