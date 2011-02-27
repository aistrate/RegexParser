using System;
using System.Collections.Generic;
using ParserCombinators;
using RegexParser.Patterns;
using RegexParser.Transforms;
using Utility.ConsLists;

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
        protected BaseMatcher(string patternText, RegexOptionsEx options)
        {
            Options = options;

            Pattern = BasePattern.CreatePattern(patternText);
            Pattern = TransformAST(Pattern);
        }

        public BasePattern Pattern { get; private set; }
        public RegexOptionsEx Options { get; private set; }

        public static BaseMatcher CreateMatcher(AlgorithmType algorithmType, string patternText, RegexOptions options)
        {
            return CreateMatcher(algorithmType, patternText, new RegexOptionsEx(options));
        }

        public static BaseMatcher CreateMatcher(AlgorithmType algorithmType, string patternText, RegexOptionsEx options)
        {
            switch (algorithmType)
            {
                case AlgorithmType.ExplicitDFA:
                    return new ExplicitDFAMatcher(patternText, options);

                case AlgorithmType.ImplicitDFA:
                    return new ImplicitDFAMatcher(patternText, options);

                case AlgorithmType.Backtracking:
                    return new BacktrackingMatcher(patternText, options);

                default:
                    throw new NotImplementedException("Algorithm not yet implemented.");
            }
        }

        protected virtual BasePattern TransformAST(BasePattern pattern)
        {
            return new RegexOptionsASTTransform(Options).Transform(pattern);
        }

        protected abstract Result<char, string> Parse(IConsList<char> consList, int length);

        public IEnumerable<Match2> GetMatches(string inputText)
        {
            IConsList<char> consList = new ArrayConsList<char>(inputText);
            int index = 0;

            while (index <= inputText.Length)
            {
                Result<char, string> result = Parse(consList, inputText.Length - index);

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

                if (!consList.IsEmpty)
                    consList = consList.Tail;

                index++;
            }
        }
    }
}
