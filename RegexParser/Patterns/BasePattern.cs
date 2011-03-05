using System;
using System.Collections.Generic;
using System.Linq;
using Utility.BaseTypes;
using Utility.ConsLists;
using Utility.PrettyPrint;

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

        public abstract PPElement ToPrettyPrint();

        public override string ToString()
        {
            return PPElement.FormatAsFlat(ToPrettyPrint());
        }

        public string FormatAsTree(int indentLevel)
        {
            return PPElement.FormatAsTree(ToPrettyPrint(), indentLevel);
        }

        protected static PPElement PPGroupWithDelimiters(IEnumerable<PPElement> children)
        {
            return PPGroupWithDelimiters(new PPText(","), children);
        }

        protected static PPElement PPGroupWithDelimiters(PPElement separator, IEnumerable<PPElement> children)
        {
            return new PPGroup(
                            new PPNewline(),
                            new PPText("{"),
                            new PPIncIndent(
                                new PPGroup(
                                    separator,
                                    children.Select<PPElement, PPElement>(c =>
                                        new PPGroup(
                                            new PPNewline(),
                                            c)))),
                            new PPNewline(),
                            new PPText("}"));
        }

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
