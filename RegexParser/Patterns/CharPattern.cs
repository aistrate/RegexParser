using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Patterns
{
    public abstract class CharPattern : BasePattern
    {
        public CharPattern()
            : base(PatternType.Char)
        {
        }

        public abstract bool IsMatch(char c);
    }
}
