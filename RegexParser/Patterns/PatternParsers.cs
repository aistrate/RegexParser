using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.Util;

namespace RegexParser.Patterns
{
    public class PatternParsers : CharParsers
    {
        static PatternParsers()
        {
            CharEscape = Either(from c in NoneOf(specialChars)
                                select (BasePattern)new CharPattern(c),

                                from b in Char('\\')
                                from esc in Choice(new[]
                                {
                                    from c in OneOf(specialChars) select new CharPattern(c),
                                    from c in Char('t') select new CharPattern('\t'),
                                    from c in Char('n') select new CharPattern('\n'),
                                    from c in Char('r') select new CharPattern('\r'),

                                    from k in Either(from c in Char('x') select 2,
                                                     from c in Char('u') select 4)
                                    from hs in Count(k, HexDigit)
                                    select new CharPattern((char)Numeric.ReadHex(hs)),

                                    from os in Count(2, 3, OctDigit)
                                    select new CharPattern((char)Numeric.ReadOct(os))
                                })
                                select (BasePattern)esc);


            charClasses();


            Quantifier = from child in Choice(new[]
                                       {
                                           Lazy(() => Group),
                                           CharEscape,
                                           CharClass
                                       })
                         from quant in Choice(new[]
                                       {
                                           from q in Char('*') select new { Min = 0, Max = (int?)null },
                                           from q in Char('+') select new { Min = 1, Max = (int?)null },
                                           from q in Char('?') select new { Min = 0, Max = (int?)1 }
                                       })
                         from greedy in Option(true,
                                               from c in Char('?') select false)
                         select (BasePattern)new QuantifierPattern(child, quant.Min, quant.Max, greedy);


            Group = Between(Char('('),
                            Char(')'),
                            Lazy(() => BareGroup));

            BareGroup = from ps in Many(Choice(new[]
                                        {
                                            Quantifier,
                                            Group,
                                            CharEscape,
                                            CharClass
                                        }))
                        select (BasePattern)new GroupPattern(ps);

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

                                from positive in Option(true,
                                                        from c in Char('^') select false)
                                from atoms in Many1(Choice(new[]
                                                    {
                                                        charRange,
                                                        singleChar
                                                    }))
                                select (BasePattern)new CharClassPattern(positive, atoms));

            CharClass = Choice(new[]
                        {
                            from c in Char('.')
                            select (BasePattern)CharClassPattern.AnyChar,

                            from b in Char('\\')
                            from cls in Choice(new[]
                            {
                                from c in Char('s') select CharClassPattern.WhitespaceChar,
                                from c in Char('S') select CharClassPattern.WhitespaceChar.Negated,
                                from c in Char('w') select CharClassPattern.WordChar,
                                from c in Char('W') select CharClassPattern.WordChar.Negated,
                                from c in Char('d') select CharClassPattern.DigitChar,
                                from c in Char('D') select CharClassPattern.DigitChar.Negated
                            })
                            select (BasePattern)cls,

                            CharGroup
                        });
        }

        public static Parser<char, BasePattern> CharEscape;

        public static Parser<char, BasePattern> CharGroup;
        public static Parser<char, BasePattern> CharClass;

        public static Parser<char, BasePattern> Quantifier;

        public static Parser<char, BasePattern> BareGroup;
        public static Parser<char, BasePattern> Group;
        public static Parser<char, BasePattern> Regex;

        private static string specialChars = @".$^{[(|)*+?\";
    }
}
