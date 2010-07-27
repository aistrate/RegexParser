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

            while (index <= InputText.Length)
            {
                Result<char, string> result = matchParser(consList);

                if (result != null)
                {
                    yield return new Match2(index, result.Value.Length, result.Value);

                    if (result.Value.Length > 0)
                    {
                        consList = result.Rest;
                        index += result.Value.Length;
                        continue;
                    }
                }

                consList = consList.Tail;
                index++;
            }
        }
    }
}
