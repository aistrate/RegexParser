using ParserCombinators;
using ParserCombinators.ConsLists;

namespace RegexParser.Matchers
{
    public class BacktrackingMatcher : BaseMatcher
    {
        public BacktrackingMatcher(string patternText)
            : base(patternText)
        {
        }

        protected override Result<char, string> Parse(IConsList<char> consList)
        {
            return null;
        }
    }
}
