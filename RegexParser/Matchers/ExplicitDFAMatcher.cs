using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using RegexParser.Patterns;

namespace RegexParser.Matchers
{
    public class ExplicitDFAMatcher : BaseMatcher
    {
        public ExplicitDFAMatcher(string patternText)
            : base(patternText)
        {
        }

        protected override Parser<char, string> CreateParser(BasePattern pattern)
        {
            return null;
        }
    }
}
