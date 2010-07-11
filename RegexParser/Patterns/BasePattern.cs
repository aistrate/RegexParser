using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.ParserCombinators.ConsLists;

namespace RegexParser.Patterns
{
    public abstract class BasePattern
    {
        public static BasePattern CreatePattern(string patternText)
        {
            return new PatternParsers().WholePattern(new LinkedConsList<char>(patternText)).Value;
        }
    }
}
