using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Pattern
{
    public abstract class BasePattern
    {
        public static BasePattern CreatePattern(string patternText)
        {
            return new GroupPattern(patternText.Select(c => new CharPattern(c))
                                               .Cast<BasePattern>());
        }
    }
}
