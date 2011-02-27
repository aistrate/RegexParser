using ParserCombinators;
using Utility.ConsLists;

namespace RegexParser.Matchers
{
    public class ImplicitDFAMatcher : BaseMatcher
    {
        public ImplicitDFAMatcher(string patternText)
            : base(patternText)
        {
        }

        protected override Result<char, string> Parse(IConsList<char> consList, int length)
        {
            return null;
        }
    }
}
