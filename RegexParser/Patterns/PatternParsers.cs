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

            WithBackslash = ch => (from b in Char('\\')
                                   from c in Char(ch)
                                   select c);

            characterClassPatterns();

            BareGroupPattern = from ps in Many(Choice(() => GroupPattern,
                                                      () => CharPattern,
                                                      () => CharacterClassPattern))
                               select (BasePattern)new GroupPattern(ps);

            GroupPattern = Between(Char('('),
                                   Char(')'),
                                   BareGroupPattern);

            WholePattern = BareGroupPattern;
        }

        private static void characterClassPatterns()
        {
            CharRangeSubgroupPattern = from frm in NoneOf("-]")
                                       from d in Char('-')
                                       from to in NoneOf("-]")
                                       select (CharSubgroup)new CharRangeSubgroup { From = frm, To = to };

            SingleCharSubgroupPattern = from c in NoneOf("-]")
                                        select (CharSubgroup)new SingleCharSubgroup { Value = c };

            CharGroupPattern = Between(Char('['),
                                       Char(']'),

                                       from isPositive in Option(true, from c in Char('^')
                                                                       select false)
                                       from subgroups in Many1(Choice(new[] { CharRangeSubgroupPattern,
                                                                              SingleCharSubgroupPattern }))
                                       let charRanges = subgroups.OfType<CharRangeSubgroup>()
                                                                 .Select(s => new CharClassPattern.CharRange(s.From, s.To))
                                       let singleChars = subgroups.OfType<SingleCharSubgroup>()
                                                                  .Select(s => s.Value)
                                       select (BasePattern)new CharClassPattern(isPositive, singleChars, charRanges));

            CharacterClassPattern = Choice(new[]
                                    {
                                        from c in Char('.')
                                        select (BasePattern)CharClassPattern.AnyCharacter,

                                        from c in WithBackslash('s')
                                        select (BasePattern)CharClassPattern.WhitespaceCharacter,

                                        from c in WithBackslash('S')
                                        select (BasePattern)CharClassPattern.WhitespaceCharacter.Negated,

                                        from c in WithBackslash('w')
                                        select (BasePattern)CharClassPattern.WordCharacter,

                                        from c in WithBackslash('W')
                                        select (BasePattern)CharClassPattern.WordCharacter.Negated,

                                        from c in WithBackslash('d')
                                        select (BasePattern)CharClassPattern.DigitCharacter,

                                        from c in WithBackslash('D')
                                        select (BasePattern)CharClassPattern.DigitCharacter.Negated,

                                        CharGroupPattern
                                    });
        }

        public static Parser<char, BasePattern> CharPattern;
        public static Func<char, Parser<char, char>> WithBackslash;

        private static Parser<char, CharSubgroup> CharRangeSubgroupPattern;
        private static Parser<char, CharSubgroup> SingleCharSubgroupPattern;
        public static Parser<char, BasePattern> CharGroupPattern;
        public static Parser<char, BasePattern> CharacterClassPattern;

        public static Parser<char, BasePattern> BareGroupPattern;
        public static Parser<char, BasePattern> GroupPattern;
        public static Parser<char, BasePattern> WholePattern;


        private static char[] specialCharacters = @".$^{[(|)*+?\".ToCharArray();

        // TODO: move these classes inside class CharClassPattern
        private interface CharSubgroup { }

        private struct CharRangeSubgroup : CharSubgroup
        {
            public char From;
            public char To;
        }

        private struct SingleCharSubgroup : CharSubgroup
        {
            public char Value;
        }
    }
}
