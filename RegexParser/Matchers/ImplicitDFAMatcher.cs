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
            Parser<char, string> matchParser = MatchParsers.CreateParser(Pattern);

            IConsList<char> consList = new ArrayConsList<char>(InputText);
            int index = 0;

            while (!consList.IsEmpty)
            {
                Result<char, string> result = matchParser(consList);

                if (result != null)
                {
                    yield return new Match2(index, result.Value.Length, result.Value);

                    consList = result.Rest;
                    index += result.Value.Length;
                }
                else
                {
                    consList = consList.Tail;
                    index++;
                }
            }
        }
    }
}
