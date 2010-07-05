using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class BacktrackingMatcher : BaseMatcher
    {
        public BacktrackingMatcher(BasePattern pattern, string input)
            : base(pattern, input)
        {
        }

        protected override IEnumerable<Match2> GetMatches()
        {
            char[] patternText = ((GroupPattern)Pattern).Patterns.Cast<CharPattern>()
                                                                 .Select(cp => cp.Value)
                                                                 .ToArray();

            for (int index = 0; index < InputText.Length; index++)
            {
                bool isMatch = true;
                int i = index;
                int length = 0;

                foreach (char p in patternText)
                    if (i < InputText.Length && p == InputText[i++])
                        length++;
                    else
                    {
                        isMatch = false;
                        break;
                    }

                if (isMatch)
                {
                    yield return new Match2(index, length, new string(patternText));
                    index = i;
                }
            }

            yield break;
        }
    }
}
