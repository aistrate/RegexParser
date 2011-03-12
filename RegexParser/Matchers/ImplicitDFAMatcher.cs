using ParserCombinators;
using Utility.ConsLists;

namespace RegexParser.Matchers
{
    public class ImplicitDFAMatcher : BaseMatcher
    {
        public ImplicitDFAMatcher(string patternText, RegexOptionsEx options)
            : base(patternText, options)
        {
        }

        protected override Result<char, string> Parse(ArrayConsList<char> consList, ArrayConsList<char> afterLastMatch)
        {
            return null;
        }
    }
}
