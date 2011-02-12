using System;
using System.Linq;
using ParserCombinators;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class ExplicitDFAMatcher : BaseMatcher
    {
        public ExplicitDFAMatcher(string patternText)
            : base(patternText)
        {
            Parser = createParser(Pattern);
        }

        protected Parser<char, string> Parser { get; private set; }

        protected override Result<char, string> Parse(IConsList<char> consList)
        {
            return Parser(consList);
        }

        private Parser<char, string> createParser(BasePattern pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException("pattern.", "Pattern is null when creating match parser.");

            if (pattern is GroupPattern)
                return from vs in CharParsers.Sequence(((GroupPattern)pattern).Patterns
                                                                              .Select(p => createParser(p)))
                       select vs.JoinStrings();

            else if (pattern is QuantifierPattern)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                return from vs in CharParsers.Count(quant.MinOccurrences, quant.MaxOccurrences, createParser(quant.ChildPattern))
                       select vs.JoinStrings();
            }

            else if (pattern is AlternationPattern)
                return CharParsers.Choice(((AlternationPattern)pattern).Alternatives
                                                                       .Select(p => createParser(p))
                                                                       .ToArray());

            else if (pattern is StringPattern)
                return CharParsers.String(((StringPattern)pattern).Value);

            else if (pattern is CharPattern)
                return from c in CharParsers.Satisfy(((CharPattern)pattern).IsMatch)
                       select new string(c, 1);

            else
                return null;
        }
    }
}
