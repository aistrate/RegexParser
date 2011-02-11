using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class ImplicitDFAMatcher : BaseMatcher
    {
        public ImplicitDFAMatcher(string patternText)
            : base(patternText)
        {
        }

        protected override Parser<char, string> CreateParser(BasePattern pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException("pattern.", "Pattern is null when creating match parser.");

            if (pattern is GroupPattern)
                return from vs in CharParsers.Sequence(((GroupPattern)pattern).Patterns
                                                                              .Select(p => CreateParser(p)))
                       select vs.JoinStrings();

            else if (pattern is QuantifierPattern)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                return from vs in CharParsers.Count(quant.MinOccurrences, quant.MaxOccurrences, CreateParser(quant.ChildPattern))
                       select vs.JoinStrings();
            }

            else if (pattern is AlternationPattern)
                return CharParsers.Choice(((AlternationPattern)pattern).Alternatives
                                                                       .Select(p => CreateParser(p))
                                                                       .ToArray());

            else if (pattern is CharPattern)
                return from c in CharParsers.Satisfy(((CharPattern)pattern).IsMatch)
                       select new string(c, 1);

            else
                return null;
        }
    }
}
