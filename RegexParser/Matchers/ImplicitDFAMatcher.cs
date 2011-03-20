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

        protected override Result<char, Match2> Parse(ArrayConsList<char> consList, int afterLastMatchIndex)
        {
            return new Result<char, Match2>(Match2.Empty, consList);
        }
    }
}
