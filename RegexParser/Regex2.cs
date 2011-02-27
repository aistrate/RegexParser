using System.Linq;
using RegexParser.Matchers;

namespace RegexParser
{
    public class Regex2
    {
        public Regex2(string patternText)
            : this(patternText, AlgorithmType.Backtracking, RegexOptions.None)
        {
        }

        public Regex2(string patternText, AlgorithmType algorithmType)
            : this(patternText, algorithmType, RegexOptions.None)
        {
        }

        public Regex2(string patternText, RegexOptions options)
            : this(patternText, AlgorithmType.Backtracking, options)
        {
        }

        public Regex2(string patternText, AlgorithmType algorithmType, RegexOptions options)
        {
            PatternText = patternText;
            AlgorithmType = algorithmType;
            Options = options;

            Matcher = BaseMatcher.CreateMatcher(AlgorithmType, PatternText, options);
        }

        public string PatternText { get; private set; }
        public AlgorithmType AlgorithmType { get; private set; }
        public RegexOptions Options { get; private set; }

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
