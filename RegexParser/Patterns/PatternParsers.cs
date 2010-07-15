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

            //(bool isPositive) => BareGroupPattern;
            //CharRangePattern = from s in NoneOf("-]")
            //                   from d in Char('-')
            //                   from e in NoneOf("-]")
            //                   select range(s, e);

            //CharGroupPattern = Between(Char('['),
            //                           Char(']'),
            //                           Many1(Choice(new[] { CharRangePattern,
            //                                                from c in NoneOf("-]")
            //                                                select new string(new[] { c }) })));

            BareGroupPattern = from ps in Many(Choice(() => GroupPattern,
                                                      () => CharPattern))
                               select (BasePattern)new GroupPattern(ps);

            GroupPattern = Between(Char('('),
                                   Char(')'),
                                   BareGroupPattern);

            WholePattern = BareGroupPattern;
        }

        public static Parser<char, BasePattern> CharPattern;
        public static Parser<char, string> CharRangePattern;
        public static Parser<char, BasePattern> CharGroupPattern;

        public static Parser<char, BasePattern> BareGroupPattern;
        public static Parser<char, BasePattern> GroupPattern;
        public static Parser<char, BasePattern> WholePattern;

        private static string range(char from, char to)
        {
            StringBuilder sb = new StringBuilder();

            for (char c = from; c <= to; c++)
                sb.Append(c);

            return sb.ToString();
        }

        private static char[] specialCharacters = @".$^{[(|)*+?\".ToCharArray();
    }
}
