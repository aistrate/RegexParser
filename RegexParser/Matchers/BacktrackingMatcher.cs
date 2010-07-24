using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class BacktrackingMatcher : BaseMatcher
    {
        public BacktrackingMatcher(BasePattern pattern, string inputText)
            : base(pattern, inputText)
        {
        }

        protected override IEnumerable<Match2> GetMatches()
        {
            CharPattern[] charPatterns = ((GroupPattern)Pattern).Patterns.Cast<CharPattern>()
                                                                         .ToArray();

            for (int index = 0; index < InputText.Length; index++)
            {
                StringBuilder match = new StringBuilder();
                bool isMatch = true;
                int i = index;
                int length = 0;

                foreach (CharPattern charPattern in charPatterns)
                    if (i < InputText.Length && charPattern.IsMatch(InputText[i++]))
                    {
                        match.Append(InputText[i - 1]);
                        length++;
                    }
                    else
                    {
                        isMatch = false;
                        break;
                    }

                if (isMatch)
                {
                    yield return new Match2(index, length, match.ToString());
                    index = i - 1;
                }
            }
        }
    }
}
