using System;
using System.Collections.Generic;
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
        protected BaseMatcher(string patternText)
        {
            Pattern = TransformPattern(BasePattern.CreatePattern(patternText));
        }

        public BasePattern Pattern { get; private set; }

        public static BaseMatcher CreateMatcher(AlgorithmType algorithmType, string patternText)
        {
            switch (algorithmType)
            {
                case AlgorithmType.ExplicitDFA:
                    return new ExplicitDFAMatcher(patternText);

                case AlgorithmType.ImplicitDFA:
                    return new ImplicitDFAMatcher(patternText);

                case AlgorithmType.Backtracking:
                    return new BacktrackingMatcher(patternText);

                default:
                    throw new NotImplementedException("Algorithm not yet implemented.");
            }
        }

        protected abstract Parser<char, string> CreateParser(BasePattern pattern);

        protected virtual BasePattern TransformPattern(BasePattern pattern)
        {
            return new StringAstTransform().Transform(pattern);
        }

        public IEnumerable<Match2> GetMatches(string inputText)
        {
            Parser<char, string> matchParser = CreateParser(Pattern);

            IConsList<char> consList = new ArrayConsList<char>(inputText);
            int index = 0;

            while (index <= inputText.Length)
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
