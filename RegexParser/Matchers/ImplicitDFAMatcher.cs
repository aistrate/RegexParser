using ParserCombinators;
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
            return null;
        }
    }
}
