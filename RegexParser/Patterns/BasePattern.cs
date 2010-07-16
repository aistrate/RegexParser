using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.ConsLists;

namespace RegexParser.Patterns
{
    public abstract class BasePattern
    {
        public static BasePattern CreatePattern(string patternText)
        {
            var result = PatternParsers.Regex(new ArrayConsList<char>(patternText));
            
            return result.Rest.IsEmpty ? result.Value : null;
        }
    }
}
