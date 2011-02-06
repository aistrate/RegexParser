using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.ConsLists;
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
            return transformCharSeq2String(pattern);
        }

        private BasePattern transformCharSeq2String(BasePattern pattern)
        {
            if (pattern is GroupPattern)
            {
                BasePattern[] oldChildPatterns = ((GroupPattern)pattern).Patterns;
                List<BasePattern> newChildPatterns = new List<BasePattern>();
                StringBuilder currentString = new StringBuilder();

                foreach (BasePattern oldChildPattern in oldChildPatterns)
                    if (oldChildPattern is CharEscapePattern)
                        currentString.Append(((CharEscapePattern)oldChildPattern).Value);
                    else
                    {
                        if (currentString.Length > 0)
                        {
                            addStringPattern(newChildPatterns, currentString.ToString());
                            currentString = new StringBuilder();
                        }

                        newChildPatterns.Add(transformCharSeq2String(oldChildPattern));
                    }

                if (currentString.Length > 0)
                    addStringPattern(newChildPatterns, currentString.ToString());

                return new GroupPattern(newChildPatterns);
            }

            else if (pattern is QuantifierPattern)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                return new QuantifierPattern(transformCharSeq2String(quant.ChildPattern),
                                             quant.MinOccurrences, quant.MaxOccurrences, quant.IsGreedy);
            }

            else if (pattern is AlternationPattern)
                return new AlternationPattern(((AlternationPattern)pattern).Alternatives
                                                                           .Select(a => transformCharSeq2String(a)));

            else
                return pattern;
        }

        private void addStringPattern(List<BasePattern> patterns, string s)
        {
            if (s.Length > 1)
                patterns.Add(new StringPattern(s));
            else
                patterns.Add(new CharEscapePattern(s[0]));
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
