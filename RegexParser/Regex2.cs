using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser
{
    public enum AlgorithmType
    {
        ExplicitDFA,
        ImplicitDFA,
        Backtracking
    }

    public class Regex2
    {
        public Regex2(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; private set; }

        public override string ToString() { return Pattern; }

        public Match2 Match(string input)
        {
            return Match2.Empty;
        }

        public MatchCollection2 Matches(string input)
        {
            return new MatchCollection2(Match(input));
        }

        public bool IsMatch(string input)
        {
            return Match(input).Success;
        }


        #region Static Methods

        public static Match2 Match(string input, string pattern)
        {
            return (new Regex2(pattern)).Match(input);
        }

        public static MatchCollection2 Matches(string input, string pattern)
        {
            return (new Regex2(pattern)).Matches(input);
        }

        public static bool IsMatch(string input, string pattern)
        {
            return (new Regex2(pattern)).IsMatch(input);
        }

        #endregion
    }
}
