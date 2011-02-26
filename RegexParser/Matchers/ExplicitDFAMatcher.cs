using System;
using System.Linq;
using ParserCombinators;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;
using RegexParser.Patterns;
using RegexParser.Transforms;

namespace RegexParser.Matchers
{
    public class ExplicitDFAMatcher : BaseMatcher
    {
        public ExplicitDFAMatcher(string patternText)
            : base(patternText)
        {
            Parser = createParser(Pattern);
        }

        protected override BasePattern TransformAST(BasePattern pattern)
        {
            return new StringASTTransform().Transform(pattern);
        }

        protected Parser<char, string> Parser { get; private set; }

        protected override Result<char, string> Parse(IConsList<char> consList, int length)
        {
            return Parser(consList);
        }

        private Parser<char, string> createParser(BasePattern pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException("pattern.", "Pattern is null when creating match parser.");

            switch (pattern.Type)
            {
                case PatternType.Group:
                    return from vs in CharParsers.Sequence(((GroupPattern)pattern).Patterns
                                                                                  .Select(p => createParser(p)))
                           select vs.JoinStrings();


                case PatternType.Quantifier:
                    QuantifierPattern quant = (QuantifierPattern)pattern;
                    return from vs in CharParsers.Count(quant.MinOccurrences, quant.MaxOccurrences, createParser(quant.ChildPattern))
                           select vs.JoinStrings();


                case PatternType.Alternation:
                    return CharParsers.Choice(((AlternationPattern)pattern).Alternatives
                                                                           .Select(p => createParser(p))
                                                                           .ToArray());


                case PatternType.String:
                    return CharParsers.String(((StringPattern)pattern).Value);


                case PatternType.Char:
                    return from c in CharParsers.Satisfy(((CharPattern)pattern).IsMatch)
                           select new string(c, 1);


                default:
                    // TODO: throw exception if pattern not parsable by DFA matcher
                    return null;
            }
        }
    }
}
