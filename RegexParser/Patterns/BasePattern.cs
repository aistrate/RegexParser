using System;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;

namespace RegexParser.Patterns
{
    public enum PatternType
    {
        Group,
        Quantifier,
        Alternation,
        Anchor,
        String,
        Char,
    }

    public abstract class BasePattern
    {
        protected BasePattern(PatternType type, int minCharLength)
        {
            Type = type;
            MinCharLength = minCharLength;
        }

        public PatternType Type { get; private set; }

        /// <summary>
        /// Minimum number of characters the pattern will need in the input string, in order to match.
        /// Calculated (recursively) based on children's value of the same property,
        /// ultimately based on CharPattern's value of 1.
        /// </summary>
        public int MinCharLength { get; private set; }

        public static BasePattern CreatePattern(string patternText)
        {
            var result = PatternParsers.Regex(new ArrayConsList<char>(patternText));

            if (result.Rest.IsEmpty)
                return result.Value;
            else
                throw new ArgumentException(
                                string.Format("Could not understand part of the regex pattern: {0}.",
                                              result.Rest.AsEnumerable().AsString().ShowVerbatim()),
                                "patternText.");
        }

#if ENABLE_ASSERT
        protected const bool isAssertEnabled = true;
#else
        protected const bool isAssertEnabled = false;
#endif
    }
}
