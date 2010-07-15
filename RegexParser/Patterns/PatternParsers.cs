using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;

namespace RegexParser.Patterns
{
    public class PatternParsers : CharParsers
    {
        static PatternParsers()
        {
            CharPattern = from c in NoneOf(specialCharacters)
                          select (BasePattern)new CharPattern(c);


            CharRangePattern = from start in NoneOf("-]")
                               from d in Char('-')
                               from end in NoneOf("-]")
                               select new CharClassPattern.CharRange(start, end);

            CharClassPattern = Between(Char('['),
                                       Char(']'),
                                       from ranges in Many1(CharRangePattern)
                                       select (BasePattern)new CharClassPattern(true, ranges));


            BareGroupPattern = from ps in Many(Choice(() => GroupPattern,
                                                      () => CharPattern,
                                                      () => CharClassPattern))
                               select (BasePattern)new GroupPattern(ps);

            GroupPattern = Between(Char('('),
                                   Char(')'),
                                   BareGroupPattern);

            WholePattern = BareGroupPattern;
        }

        public static Parser<char, BasePattern> CharPattern;
        public static Parser<char, CharClassPattern.CharRange> CharRangePattern;
        public static Parser<char, BasePattern> CharClassPattern;

        public static Parser<char, BasePattern> BareGroupPattern;
        public static Parser<char, BasePattern> GroupPattern;
        public static Parser<char, BasePattern> WholePattern;

        private static char[] specialCharacters = @".$^{[(|)*+?\".ToCharArray();
    }
}
