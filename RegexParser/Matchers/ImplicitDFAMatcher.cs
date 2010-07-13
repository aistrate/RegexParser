using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.ConsLists;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class ImplicitDFAMatcher : BaseMatcher
    {
        public ImplicitDFAMatcher(BasePattern pattern, string inputText)
            : base(pattern, inputText)
        {
        }

        protected override IEnumerable<Match2> GetMatches()
        {
            Parser<char, string> matchParser = new MatchParsers().CreateParser(Pattern);
            IConsList<char> consList = new ArrayConsList<char>(InputText);
            
            int index = 0;
            for (IConsList<char> partialList = consList; !partialList.IsEmpty; partialList = partialList.Tail, index++)
            {
                Result<char, string> result = matchParser(partialList);

                if (result != null)
                {
                    yield return new Match2(index, result.Value.Length, result.Value);

                    partialList = partialList.Skip(result.Value.Length - 1);
                    index += result.Value.Length - 1;
                }
            }
        }
    }
}
