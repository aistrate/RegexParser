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
            var charRangeAtom = from frm in NoneOf("-]")
                                from d in Char('-')
                                from to in NoneOf("-]")
                                select (CharClassPattern.ICharClassAtom)new CharClassPattern.CharRange(frm, to);

            var singleCharAtom = from c in NoneOf("-]")
                                 select (CharClassPattern.ICharClassAtom)new CharClassPattern.SingleChar(c);

            CharGroupPattern = Between(Char('['),
                                       Char(']'),

                                       from isPositive in Option(true, from c in Char('^')
                                                                       select false)
                                       from atoms in Many1(Choice(new[] { charRangeAtom,
                                                                          singleCharAtom }))
                                       select (BasePattern)new CharClassPattern(isPositive, atoms));

            CharacterClassPattern = Choice(new[]
                                    {
                                        from c in Char('.')
                                        select (BasePattern)CharClassPattern.AnyChar,

                                        from c in WithBackslash('s')
                                        select (BasePattern)CharClassPattern.WhitespaceChar,

                                        from c in WithBackslash('S')
                                        select (BasePattern)CharClassPattern.WhitespaceChar.Negated,

                                        from c in WithBackslash('w')
                                        select (BasePattern)CharClassPattern.WordChar,

                                        from c in WithBackslash('W')
                                        select (BasePattern)CharClassPattern.WordChar.Negated,

                                        from c in WithBackslash('d')
                                        select (BasePattern)CharClassPattern.DigitChar,

                                        from c in WithBackslash('D')
                                        select (BasePattern)CharClassPattern.DigitChar.Negated,

                                        CharGroupPattern
                                    });
        }

        public static Parser<char, BasePattern> CharPattern;
        public static Func<char, Parser<char, char>> WithBackslash;

        public static Parser<char, BasePattern> CharGroupPattern;
        public static Parser<char, BasePattern> CharacterClassPattern;

        public static Parser<char, BasePattern> BareGroupPattern;
        public static Parser<char, BasePattern> GroupPattern;
        public static Parser<char, BasePattern> WholePattern;

        private static char[] specialCharacters = @".$^{[(|)*+?\".ToCharArray();
    }
}
