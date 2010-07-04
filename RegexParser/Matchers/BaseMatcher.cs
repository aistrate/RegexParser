using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public enum AlgorithmType
    {
        ExplicitDFA,
        ImplicitDFA,
        Backtracking
    }

    public abstract class BaseMatcher
    {
        protected BaseMatcher(BasePattern pattern, string input)
        {
            Pattern = pattern;
            InputText = input.ToCharArray();
        }

        protected BasePattern Pattern { get; private set; }
        protected char[] InputText { get; private set; }

        public static BaseMatcher CreateMatcher(AlgorithmType algorithmType, BasePattern pattern, string input)
        {
            switch (algorithmType)
            {
                case AlgorithmType.Backtracking:
                    return new BacktrackingMatcher(pattern, input);
                
                default:
                    throw new NotImplementedException("Algorithm not yet implemented.");
            }
        }

        public MatchCollection2 Matches
        {
            get
            {
                if (matches == null)
                    matches = new MatchCollection2(GetMatches());
                return matches;
            }
        }
        private MatchCollection2 matches;

        protected abstract IEnumerable<Match2> GetMatches();
    }
}
