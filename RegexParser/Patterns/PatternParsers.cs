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
            characterClassParsers();

            CharPattern = from c in NoneOf(specialCharacters)
                          select (BasePattern)new CharPattern(c);

            BareGroupPattern = from ps in Many(Choice(() => GroupPattern,
                                                      () => CharPattern,
                                                      () => CharClassPattern))
                               select (BasePattern)new GroupPattern(ps);

            GroupPattern = Between(Char('('),
                                   Char(')'),
                                   BareGroupPattern);

            WholePattern = BareGroupPattern;
        }

        private static void characterClassParsers()
        {
            CharRangeSubclassPattern = from frm in NoneOf("-]")
                                       from d in Char('-')
                                       from to in NoneOf("-]")
                                       select (CharSubclass)new CharRangeSubclass { From = frm, To = to };

            SingleCharSubclassPattern = from c in NoneOf("-]")
                                        select (CharSubclass)new SingleCharSubclass { Value = c };

            CharClassPattern = Between(Char('['),
                                       Char(']'),

                                       from isPositive in Option(true, from c in Char('^')
                                                                       select false)
                                       from subclasses in Many1(Choice(new[] { CharRangeSubclassPattern,
                                                                               SingleCharSubclassPattern }))
                                       let charRanges = subclasses.OfType<CharRangeSubclass>()
                                                                  .Select(s => new CharClassPattern.CharRange(s.From, s.To))
                                       let singleChars = subclasses.OfType<SingleCharSubclass>()
                                                                   .Select(s => s.Value)
                                       select (BasePattern)new CharClassPattern(isPositive, singleChars, charRanges));
        }

        private static Parser<char, CharSubclass> CharRangeSubclassPattern;
        private static Parser<char, CharSubclass> SingleCharSubclassPattern;
        public static Parser<char, BasePattern> CharClassPattern;

        public static Parser<char, BasePattern> CharPattern;
        
        public static Parser<char, BasePattern> BareGroupPattern;
        public static Parser<char, BasePattern> GroupPattern;
        public static Parser<char, BasePattern> WholePattern;


        private static char[] specialCharacters = @".$^{[(|)*+?\".ToCharArray();

        private interface CharSubclass { }

        private struct CharRangeSubclass : CharSubclass
        {
            public char From;
            public char To;
        }

        private struct SingleCharSubclass : CharSubclass
        {
            public char Value;
        }
    }
}
