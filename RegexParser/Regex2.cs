using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Matchers;
using RegexParser.Patterns;

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
        }

        public string PatternText { get; private set; }
        public AlgorithmType AlgorithmType { get; private set; }

        protected BasePattern Pattern
        {
            get
            {
                if (pattern == null)
                    pattern = BasePattern.CreatePattern(PatternText);
                return pattern;
            }
        }
        private BasePattern pattern;
        
        public override string ToString() { return PatternText; }

        public Match2 Match(string input)
        {
            return Matches(input).FirstOrDefault() ?? Match2.Empty;
        }

        public MatchCollection2 Matches(string input)
        {
            return BaseMatcher.CreateMatcher(AlgorithmType, Pattern, input).Matches;
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
