using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class ExplicitDFAMatcher : BaseMatcher
    {
        public ExplicitDFAMatcher(BasePattern pattern, string inputText)
            : base(pattern, inputText)
        {
        }

        protected override IEnumerable<Match2> GetMatches()
        {
            return new Match2[] {};
        }
    }
}
