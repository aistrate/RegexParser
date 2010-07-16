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
            BackslashedChar = ch => (from b in Char('\\')
                                     from c in Char(ch)
                                     select c);

            NormalChar = from c in NoneOf(specialChars)
                         select (BasePattern)new CharPattern(c);

            charClasses();

            BareGroup = from ps in Many(Choice(() => Group,
                                               () => NormalChar,
                                               () => CharClass))
                        select (BasePattern)new GroupPattern(ps);

            Group = Between(Char('('),
                            Char(')'),
                            BareGroup);

            Regex = BareGroup;
        }

        private static void charClasses()
        {
            var charRange = from frm in NoneOf("-]")
                            from d in Char('-')
                            from to in NoneOf("-]")
                            select (CharClassPattern.ICharClassAtom)new CharClassPattern.CharRange(frm, to);

            var singleChar = from c in NoneOf("-]")
                             select (CharClassPattern.ICharClassAtom)new CharClassPattern.SingleChar(c);

            CharGroup = Between(Char('['),
                                Char(']'),

                                from isPositive in Option(true, from c in Char('^')
                                                                select false)
                                from atoms in Many1(Choice(new[] { charRange, singleChar }))
                                select (BasePattern)new CharClassPattern(isPositive, atoms));

            CharClass = Choice(new[]
                               {
                                    from c in Char('.')
                                    select (BasePattern)CharClassPattern.AnyChar,

                                    from c in BackslashedChar('s')
                                    select (BasePattern)CharClassPattern.WhitespaceChar,

                                    from c in BackslashedChar('S')
                                    select (BasePattern)CharClassPattern.WhitespaceChar.Negated,

                                    from c in BackslashedChar('w')
                                    select (BasePattern)CharClassPattern.WordChar,

                                    from c in BackslashedChar('W')
                                    select (BasePattern)CharClassPattern.WordChar.Negated,

                                    from c in BackslashedChar('d')
                                    select (BasePattern)CharClassPattern.DigitChar,

                                    from c in BackslashedChar('D')
                                    select (BasePattern)CharClassPattern.DigitChar.Negated,

                                    CharGroup
                               });
        }

        public static Func<char, Parser<char, char>> BackslashedChar;
        public static Parser<char, BasePattern> NormalChar;

        public static Parser<char, BasePattern> CharGroup;
        public static Parser<char, BasePattern> CharClass;

        public static Parser<char, BasePattern> BareGroup;
        public static Parser<char, BasePattern> Group;
        public static Parser<char, BasePattern> Regex;

        private static string specialChars = @".$^{[(|)*+?\";
    }
}
