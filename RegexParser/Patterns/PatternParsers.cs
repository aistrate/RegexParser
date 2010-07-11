using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.ParserCombinators;

namespace RegexParser.Patterns
{
    public class PatternParsers : CharParsers
    {
        public PatternParsers()
        {
            CharPattern = from c in NoneOf(specialCharacters)
                          select (BasePattern)new CharPattern(c);

            BareGroupPattern = from ps in Many(Choice(allPatterns))
                               select (BasePattern)new GroupPattern(ps);

            GroupPattern = Between(Char('('), Char(')'), BareGroupPattern);

            WholePattern = BareGroupPattern;
        }

        public Parser<char, BasePattern> CharPattern;
        public Parser<char, BasePattern> BareGroupPattern;
        public Parser<char, BasePattern> GroupPattern;
        public Parser<char, BasePattern> WholePattern;

        private IEnumerable<Parser<char, BasePattern>> allPatterns
        {
            get
            {
                yield return GroupPattern;
                yield return CharPattern;
            }
        }

        private char[] specialCharacters = @".$^{[(|)*+?\".ToCharArray();
    }
}
