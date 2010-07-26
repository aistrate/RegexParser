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
            // Character Escapes
            CharEscape = (specialChars, charEscapeKeys) =>
                            Either(from c in NoneOf(specialChars)
                                   select new CharEscapePattern(c),

                                   from b in Char('\\')
                                   from esc in
                                       Choice(
                                           OneOf(specialChars),

                                           from c in OneOf(charEscapeKeys)
                                           select charEscapes[c],

                                           from k in
                                               Either(from c in Char('x') select 2,
                                                      from c in Char('u') select 4)
                                           from hs in Count(k, HexDigit)
                                           select (char)Numeric.ReadHex(hs),

                                           from os in Count(2, 3, OctDigit)
                                           select (char)Numeric.ReadOct(os),

                                           Satisfy(c => !char.IsLetterOrDigit(c) && c != '_'))
                                   select new CharEscapePattern(esc));

            CharEscapeOutsideClass = CharEscape(specialCharsOutsideClass,
                                                charEscapeKeysOutsideClass);

            CharEscapeInsideClass = isFirst => CharEscape(isFirst ? specialCharsInsideClass_FirstPos :
                                                                    specialCharsInsideClass,
                                                          charEscapeKeysInsideClass);


            // Character Classes
            CharRange = isFirst => from frm in CharEscapeInsideClass(isFirst)
                                   from d in Char('-')
                                   from to in CharEscapeInsideClass(false)
                                   select new CharRangePattern(frm.Value, to.Value);

            NamedCharClass = from b in Char('\\')
                             from cls in
                                 Choice(
                                     from c in Char('s') select CharClassPattern.WhitespaceChar,
                                     from c in Char('S') select CharClassPattern.WhitespaceChar.Negated,
                                     from c in Char('w') select CharClassPattern.WordChar,
                                     from c in Char('W') select CharClassPattern.WordChar.Negated,
                                     from c in Char('d') select CharClassPattern.DigitChar,
                                     from c in Char('D') select CharClassPattern.DigitChar.Negated)
                             select cls;

            GroupCharClassElement = isFirst => Choice(from p in NamedCharClass select (CharPattern)p,
                                                      from p in CharRange(isFirst) select (CharPattern)p,
                                                      from p in CharEscapeInsideClass(isFirst) select (CharPattern)p);

            GroupCharClass = Between(Char('['),
                                     Char(']'),

                                     from positive in
                                         Option(true, from c in Char('^')
                                                      select false)
                                     from first in GroupCharClassElement(true)
                                     from rest in Many(GroupCharClassElement(false))
                                     let childPatterns = new[] { first }.Concat(rest)
                                     select new CharClassPattern(positive, childPatterns));

            CharClass = Choice(from c in Char('.') select CharClassPattern.AnyChar,
                               NamedCharClass,
                               GroupCharClass);


            // Quantifiers
            Natural = from ds in Many1(Digit)
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
                                 from p in Lazy(() => Group) select (BasePattern)p,
                                 from p in CharEscapeOutsideClass select (BasePattern)p,
                                 from p in CharClass select (BasePattern)p)
                         from suffix in QuantifierSuffix
                         select new QuantifierPattern(child, suffix.Min, suffix.Max, suffix.Greedy);


            // Groups
            Group = Between(Char('('),
                            Char(')'),
                            Lazy(() => BareGroup));

            BareGroup = from ps in Many(Choice(
                                            from p in Quantifier select (BasePattern)p,
                                            from p in Group select (BasePattern)p,
                                            from p in CharEscapeOutsideClass select (BasePattern)p,
                                            from p in CharClass select (BasePattern)p))
                        select new GroupPattern(ps);

            Regex = BareGroup;
        }


        public static Func<string, string, Parser<char, CharEscapePattern>> CharEscape;
        public static Parser<char, CharEscapePattern> CharEscapeOutsideClass;
        public static Func<bool, Parser<char, CharEscapePattern>> CharEscapeInsideClass;

        public static Func<bool, Parser<char, CharRangePattern>> CharRange;
        public static Parser<char, CharClassPattern> NamedCharClass;
        public static Func<bool, Parser<char, CharPattern>> GroupCharClassElement;
        public static Parser<char, CharClassPattern> GroupCharClass;
        public static Parser<char, CharClassPattern> CharClass;

        public static Parser<char, int> Natural;
        public static Parser<char, QuantifierPattern> Quantifier;

        public static Parser<char, GroupPattern> BareGroup;
        public static Parser<char, GroupPattern> Group;
        public static Parser<char, GroupPattern> Regex;


        private static Dictionary<char, char> charEscapes = new Dictionary<char, char>()
        {
            { 'a', '\a' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' },
            { 'v', '\v' }
        };

        private const string specialCharsOutsideClass = ".$^{[(|)*+?\\";
        private const string specialCharsInsideClass_FirstPos  = "\\";
        private const string specialCharsInsideClass = "]\\";

        private static string charEscapeKeysOutsideClass = new string(charEscapes.Keys.Except("b").ToArray());
        private static string charEscapeKeysInsideClass  = new string(charEscapes.Keys.ToArray());
    }
}
