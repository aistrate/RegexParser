using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.ConsLists;
using RegexParser.Patterns;
using RegexParser.Transforms;

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
            Pattern = TransformPattern(pattern);
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

        protected abstract Parser<char, string> CreateParser(BasePattern pattern);

        protected virtual BasePattern TransformPattern(BasePattern pattern)
        {
            return new StringTransform().RunTransform(pattern);
        }

        public MatchCollection2 Matches
        {
            get
            {
                if (matches == null)
                    matches = new MatchCollection2(getMatches());
                return matches;
            }
        }
        private MatchCollection2 matches;

        private IEnumerable<Match2> getMatches()
        {
            Parser<char, string> matchParser = CreateParser(Pattern);

            IConsList<char> consList = new ArrayConsList<char>(InputText);
            int index = 0;

            while (index <= InputText.Length)
            {
                Result<char, string> result = matchParser(consList);

                if (result != null)
                {
                    yield return new Match2(index, result.Value.Length, result.Value);

                    if (result.Value.Length > 0)
                    {
                        consList = result.Rest;
                        index += result.Value.Length;
                        continue;
                    }
                }

                consList = consList.Tail;
                index++;
            }
        }
    }
}
