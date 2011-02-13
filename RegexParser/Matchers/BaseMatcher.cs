using System;
using System.Collections.Generic;
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
        protected BaseMatcher(string patternText)
        {
            Pattern = BasePattern.CreatePattern(patternText);
            Pattern = TransformAST(Pattern);
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

        protected virtual BasePattern TransformAST(BasePattern pattern)
        {
            return pattern;
        }

        protected abstract Result<char, string> Parse(IConsList<char> consList);

        public IEnumerable<Match2> GetMatches(string inputText)
        {
            IConsList<char> consList = new ArrayConsList<char>(inputText);
            int index = 0;

            while (index <= inputText.Length)
            {
                Result<char, string> result = Parse(consList);

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
