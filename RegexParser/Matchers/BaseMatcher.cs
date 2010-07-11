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
        protected BaseMatcher(BasePattern pattern, string inputText)
        {
            Pattern = pattern;
            InputText = inputText;
        }

        protected BasePattern Pattern { get; private set; }
        protected string InputText { get; private set; }

        public static BaseMatcher CreateMatcher(AlgorithmType algorithmType, BasePattern pattern, string inputText)
        {
            switch (algorithmType)
            {
                case AlgorithmType.ExplicitDFA:
                    return new ExplicitDFAMatcher(pattern, inputText);

                case AlgorithmType.ImplicitDFA:
                    return new ImplicitDFAMatcher(pattern, inputText);

                case AlgorithmType.Backtracking:
                    return new BacktrackingMatcher(pattern, inputText);

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
