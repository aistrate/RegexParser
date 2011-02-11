using System.Linq;
using RegexParser.Matchers;

namespace RegexParser
{
    public class Regex2
    {
        public Regex2(string patternText)
            : this(patternText, AlgorithmType.Backtracking)
        {
        }

        public Regex2(string patternText, AlgorithmType algorithmType)
        {
            PatternText = patternText;
            AlgorithmType = algorithmType;

            Matcher = BaseMatcher.CreateMatcher(AlgorithmType, PatternText);
        }

        public string PatternText { get; private set; }
        public AlgorithmType AlgorithmType { get; private set; }

        public BaseMatcher Matcher { get; private set; }

        public override string ToString() { return PatternText; }

        public Match2 Match(string input)
        {
            return Matches(input).FirstOrDefault() ?? Match2.Empty;
        }

        public MatchCollection2 Matches(string input)
        {
            return new MatchCollection2(Matcher.GetMatches(input));
        }

        public bool IsMatch(string input)
        {
            return Match(input).Success;
        }


        #region Static Methods

        public static Match2 Match(string input, string patternText)
        {
            return new Regex2(patternText).Match(input);
        }

        public static MatchCollection2 Matches(string input, string patternText)
        {
            return new Regex2(patternText).Matches(input);
        }

        public static bool IsMatch(string input, string patternText)
        {
            return new Regex2(patternText).IsMatch(input);
        }

        #endregion
    }
}
