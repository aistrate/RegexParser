using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Patterns
{
    public abstract class CharPattern : BasePattern
    {
        public abstract bool IsMatch(char c);

        public override PatternType Type { get { return PatternType.Char; } }
    }
}
