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
            // Characters
            CharEscape = Either(from c in NoneOf(specialChars)
                                select (BasePattern)new CharEscapePattern(c),

                                from b in Char('\\')
                                from esc in
                                    Choice(
                                        from c in OneOf(specialChars) select new CharEscapePattern(c),
                                        from c in Char('t') select new CharEscapePattern('\t'),
                                        from c in Char('n') select new CharEscapePattern('\n'),
                                        from c in Char('r') select new CharEscapePattern('\r'),

                                        from k in
                                            Either(from c in Char('x') select 2,
                                                   from c in Char('u') select 4)
                                        from hs in Count(k, HexDigit)
                                        select new CharEscapePattern((char)Numeric.ReadHex(hs)),

                                        from os in Count(2, 3, OctDigit)
                                        select new CharEscapePattern((char)Numeric.ReadOct(os)))
                                select (BasePattern)esc);

            var charRange = from frm in NoneOf("-]")
                            from d in Char('-')
                            from to in NoneOf("-]")
                            select (CharClassPattern.ICharClassAtom)new CharClassPattern.CharRange(frm, to);

            var singleChar = from c in NoneOf("-]")
                             select (CharClassPattern.ICharClassAtom)new CharClassPattern.SingleChar(c);

            CharGroup = Between(Char('['),
                                Char(']'),

                                from positive in
                                    Option(true, from c in Char('^')
                                                 select false)
                                from atoms in Many1(Choice(charRange, singleChar))
                                select (BasePattern)new CharClassPattern(positive, atoms));

            CharClass = Choice(
                            from c in Char('.')
                            select (BasePattern)CharClassPattern.AnyChar,

                            from b in Char('\\')
                            from cls in
                                Choice(
                                    from c in Char('s') select CharClassPattern.WhitespaceChar,
                                    from c in Char('S') select CharClassPattern.WhitespaceChar.Negated,
                                    from c in Char('w') select CharClassPattern.WordChar,
                                    from c in Char('W') select CharClassPattern.WordChar.Negated,
                                    from c in Char('d') select CharClassPattern.DigitChar,
                                    from c in Char('D') select CharClassPattern.DigitChar.Negated)
                            select (BasePattern)cls,

                            CharGroup);


            // Quantifiers
            var Natural = from ds in Many1(Digit)
                          select Numeric.ReadDec(ds);

            var RangeQuantifierSuffix = Between(Char('{'),
                                                Char('}'),

                                                from min in Natural
                                                from max in
                                                    Option((int?)min, from comma in Char(',')
                                                                      from m in OptionNullable(Natural)
                                                                      select m)
                                                select new { Min = min, Max = max });

            var QuantifierSuffix = from quant in
                                       Choice(
                                           from q in Char('*') select new { Min = 0, Max = (int?)null },
                                           from q in Char('+') select new { Min = 1, Max = (int?)null },
                                           from q in Char('?') select new { Min = 0, Max = (int?)1 },
                                           RangeQuantifierSuffix)
                                   from greedy in
                                       Option(true, from c in Char('?')
                                                    select false)
                                   select new { Min = quant.Min, Max = quant.Max, Greedy = greedy };

            Quantifier = from child in
                             Choice(
                                 Lazy(() => Group),
                                 CharEscape,
                                 CharClass)
                         from suffix in QuantifierSuffix
                         select (BasePattern)new QuantifierPattern(child, suffix.Min, suffix.Max, suffix.Greedy);


            // Groups
            Group = Between(Char('('),
                            Char(')'),
                            Lazy(() => BareGroup));

            BareGroup = from ps in Many(Choice(Quantifier,
                                               Group,
                                               CharEscape,
                                               CharClass))
                        select (BasePattern)new GroupPattern(ps);

            Regex = BareGroup;
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
