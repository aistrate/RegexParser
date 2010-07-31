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
                            Choice(from c in NoneOf(specialChars)
                                   select new CharEscapePattern(c),

                                   from b in Char('\\')
                                   from esc in
                                       Choice(
                                           OneOf(specialChars),

                                           from c in OneOf(charEscapeKeys)
                                           select charEscapes[c],

                                           from k in
                                               Choice(from c in Char('x') select 2,
                                                      from c in Char('u') select 4)
                                           from hs in Count(k, HexDigit)
                                           select (char)Numeric.ReadHex(hs),

                                           from os in Count(2, 3, OctDigit)
                                           select (char)Numeric.ReadOct(os),

                                           Satisfy(c => !char.IsLetterOrDigit(c) && c != '_'))
                                   select new CharEscapePattern(esc));

            CharEscapeOutsideClass = CharEscape(specialCharsOutsideClass,
                                                charEscapeKeysOutsideClass);

            CharEscapeInsideClass = (isFirstPos, isSubtract, isAfterDash) =>
                                        CharEscape(getSpecialCharsInsideClass(isFirstPos, isSubtract, isAfterDash),
                                                   charEscapeKeysInsideClass);


            // Character Classes
            NamedCharClass = from b in Char('\\')
                             from cls in OneOf(namedCharClassKeys)
                             select namedCharClasses[cls];

            CharRange = (isFirstPos, isSubtract) =>
                            from frm in CharEscapeInsideClass(isFirstPos, isSubtract, false)
                            from d in Char('-')
                            from to in CharEscapeInsideClass(false, isSubtract, true)
                            select new CharRangePattern(frm.Value, to.Value);

            CharGroupElement = (isFirstPos, isSubtract) =>
                                    Choice(from p in NamedCharClass select (CharPattern)p,
                                           from p in CharRange(isFirstPos, isSubtract) select (CharPattern)p,
                                           from p in CharEscapeInsideClass(isFirstPos, isSubtract, false) select (CharPattern)p);

            BareCharGroup = isSubtract =>
                                from positive in
                                    Option(true, from c in Char('^')
                                                 select false)
                                from first in CharGroupElement(true, isSubtract)
                                from rest in Many(CharGroupElement(false, isSubtract))
                                let childPatterns = new[] { first }.Concat(rest)
                                select (CharClassPattern)new CharGroupPattern(positive, childPatterns);

            CharClassSubtract = from baseGrp in BareCharGroup(true)
                                from d in Char('-')
                                from excludedGrp in CharGroup
                                select (CharClassPattern)new CharClassSubtractPattern(baseGrp, excludedGrp);

            CharGroup = Between(Char('['),
                                Char(']'),
                                Choice(CharClassSubtract,
                                       BareCharGroup(false)));

            CharClass = Choice(from c in Char('.') select (CharClassPattern)CharGroupPattern.AnyChar,
                               NamedCharClass,
                               CharGroup);


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


            // Alternations
            AlternationGroup = from ps in
                                   Many(Choice(
                                            from p in Quantifier select (BasePattern)p,
                                            from p in Lazy(() => Group) select (BasePattern)p,
                                            from p in CharEscapeOutsideClass select (BasePattern)p,
                                            from p in CharClass select (BasePattern)p))
                               select ps.Count() == 1 ? ps.First() :
                                                        (BasePattern)new GroupPattern(ps);

            Alternation = from alts in SepBy(2, AlternationGroup, Char('|'))
                          select new AlternationPattern(alts);


            // Groups
            BareGroup = from ps in Many(Choice(
                                            from p in Alternation select (BasePattern)p,
                                            from p in Quantifier select (BasePattern)p,
                                            from p in Lazy(() => Group) select (BasePattern)p,
                                            from p in CharEscapeOutsideClass select (BasePattern)p,
                                            from p in CharClass select (BasePattern)p))
                        select new GroupPattern(ps);

            Group = Between(Char('('),
                            Char(')'),
                            BareGroup);

            Regex = BareGroup;
        }


        public static Func<string, string, Parser<char, CharEscapePattern>> CharEscape;
        public static Parser<char, CharEscapePattern> CharEscapeOutsideClass;
        public static Func<bool, bool, bool, Parser<char, CharEscapePattern>> CharEscapeInsideClass;

        public static Parser<char, CharClassPattern> NamedCharClass;
        public static Func<bool, bool, Parser<char, CharRangePattern>> CharRange;
        public static Func<bool, bool, Parser<char, CharPattern>> CharGroupElement;
        public static Func<bool, Parser<char, CharClassPattern>> BareCharGroup;
        public static Parser<char, CharClassPattern> CharClassSubtract;
        public static Parser<char, CharClassPattern> CharGroup;
        public static Parser<char, CharClassPattern> CharClass;

        public static Parser<char, int> Natural;
        public static Parser<char, QuantifierPattern> Quantifier;

        public static Parser<char, BasePattern> AlternationGroup;
        public static Parser<char, AlternationPattern> Alternation;

        public static Parser<char, GroupPattern> BareGroup;
        public static Parser<char, GroupPattern> Group;
        public static Parser<char, GroupPattern> Regex;


        private const string specialCharsOutsideClass = ".$^{[(|)*+?\\";

        private const string specialCharsInsideClass = "]\\";
        private const string specialCharsInsideClass_FirstPos = "\\";
        private const string specialCharsInsideClass_Subtract = "-]\\";
        private const string specialCharsInsideClass_Subtract_AfterDash = "[]\\";

        private static string getSpecialCharsInsideClass(bool isFirstPos, bool isSubtract, bool isAfterDash)
        {
            return isFirstPos  ? specialCharsInsideClass_FirstPos :
                   !isSubtract ? specialCharsInsideClass :
                   isAfterDash ? specialCharsInsideClass_Subtract_AfterDash :
                                 specialCharsInsideClass_Subtract;
        }


        private static Dictionary<char, char> charEscapes =
            new Dictionary<char, char>()
            {
                { 'a', '\a' },
                { 'b', '\b' },
                { 'f', '\f' },
                { 'n', '\n' },
                { 'r', '\r' },
                { 't', '\t' },
                { 'v', '\v' },
            };

        private static string charEscapeKeysOutsideClass = charEscapes.Keys.Except("b").AsString();
        private static string charEscapeKeysInsideClass = charEscapes.Keys.AsString();


        private static Dictionary<char, CharClassPattern> namedCharClasses =
            new Dictionary<char, CharClassPattern>()
            {
                { 's', CharGroupPattern.WhitespaceChar },
                { 'S', CharGroupPattern.WhitespaceChar.Negated },
                { 'w', CharGroupPattern.WordChar },
                { 'W', CharGroupPattern.WordChar.Negated },
                { 'd', CharGroupPattern.DigitChar },
                { 'D', CharGroupPattern.DigitChar.Negated },
            };

        private static string namedCharClassKeys = namedCharClasses.Keys.AsString();
    }
}
