using System;
using System.Collections.Generic;
using System.Linq;
using ParserCombinators;
using Utility.BaseTypes;

namespace RegexParser.Patterns
{
    public class PatternParsers : CharParsers
    {
        static PatternParsers()
        {
            // Character Escapes
            CharEscape = (specialChars, charEscapeKeys) =>
                                from esc in Choice(
                                    NoneOf(specialChars),

                                    PrefixedBy(
                                        Char('\\'),
                                        Choice(
                                            OneOf(specialChars),

                                            from c in OneOf(charEscapeKeys)
                                            select charEscapes[c],

                                            from k in
                                               Choice(from _c in Char('x') select 2,
                                                      from _c in Char('u') select 4)
                                            from hs in Count(k, HexDigit)
                                            select (char)Numeric.ReadHex(hs),

                                            from os in Count(2, 3, OctDigit)
                                            select (char)Numeric.ReadOct(os),

                                            Satisfy(c => !char.IsLetterOrDigit(c) && c != '_'))))
                                select new CharEscapePattern(esc);

            CharEscapeOutsideClass = CharEscape(specialCharsOutsideClass,
                                                charEscapeKeysOutsideClass)
                                        .Cast<CharEscapePattern, BasePattern>();

            CharEscapeInsideClass = (isFirstPos, isSubtract, isAfterDash) =>
                                        CharEscape(getSpecialCharsInsideClass(isFirstPos, isSubtract, isAfterDash),
                                                   charEscapeKeysInsideClass);


            // Character Classes
            AnyCharClass = from _c in Char('.')
                           select (CharClassPattern)new AnyCharPattern(false);

            NamedCharClass = from cls in PrefixedBy(Char('\\'),
                                                    OneOf(namedCharClassKeys))
                             select namedCharClasses[cls];

            CharRange = (isFirstPos, isSubtract) =>
                            from frm in CharEscapeInsideClass(isFirstPos, isSubtract, false)
                            from _d  in Char('-')
                            from to  in CharEscapeInsideClass(false, isSubtract, true)
                            select (CharPattern)new CharRangePattern(frm.Value, to.Value);

            CharGroupElement = (isFirstPos, isSubtract) =>
                                    Choice(NamedCharClass
                                                .Cast<CharClassPattern, CharPattern>(),
                                           CharRange(isFirstPos, isSubtract),
                                           CharEscapeInsideClass(isFirstPos, isSubtract, false)
                                                .Cast<CharEscapePattern, CharPattern>());

            BareCharGroup = isSubtract =>
                                from positive in
                                    Option(true, from _c in Char('^')
                                                 select false)
                                from first in CharGroupElement(true, isSubtract)
                                from rest in Many(CharGroupElement(false, isSubtract))
                                let childPatterns = new[] { first }.Concat(rest)
                                select (CharClassPattern)new CharGroupPattern(positive, childPatterns);

            CharClassSubtract = from baseGrp in BareCharGroup(true)
                                from _d in Char('-')
                                from excludedGrp in CharGroup
                                select (CharClassPattern)new CharClassSubtractPattern(baseGrp, excludedGrp);

            CharGroup = Between(Char('['),
                                Char(']'),
                                Choice(CharClassSubtract,
                                       BareCharGroup(false)));

            CharClass = Choice(AnyCharClass,
                               NamedCharClass,
                               CharGroup)
                            .Cast<CharClassPattern, BasePattern>();


            // Anchors
            Anchor = from a in Choice(OneOf(bareAnchorKeys),
                                      PrefixedBy(Char('\\'),
                                                 OneOf(backslashAnchorKeys)))
                     select (BasePattern)new AnchorPattern(anchorTypes[a]);


            // Atomic (non-backtracking) elements
            Atom = Choice(Lazy(() => ParenGroup),
                          Anchor,
                          CharEscapeOutsideClass,
                          CharClass);


            // Quantifiers
            NaturalNum = from ds in Many1(Digit)
                         select Numeric.ReadDec(ds);

            var RangeQuantifierSuffix = Between(Char('{'),
                                                Char('}'),

                                                from min in NaturalNum
                                                from max in
                                                    Option(min, PrefixedBy(Char(','),
                                                                           Option(null, Nullable(NaturalNum))))
                                                select new { Min = min, Max = max });

            var QuantifierSuffix = from quant in
                                       Choice(
                                           from _q in Char('*') select new { Min = 0, Max = (int?)null },
                                           from _q in Char('+') select new { Min = 1, Max = (int?)null },
                                           from _q in Char('?') select new { Min = 0, Max = (int?)1 },
                                           RangeQuantifierSuffix)
                                   from greedy in
                                       Option(true, from _c in Char('?')
                                                    select false)
                                   select new { Min = quant.Min, Max = quant.Max, Greedy = greedy };

            Quantifier = from child in Atom
                         from suffix in QuantifierSuffix
                         select (BasePattern)new QuantifierPattern(child, suffix.Min, suffix.Max, suffix.Greedy);


            // Alternations
            AlternationBranch = from ps in Many(Choice(Quantifier,
                                                       Atom))
                                select ps.Count() == 1 ?
                                            ps.First() :
                                            (BasePattern)new GroupPattern(false, ps);

            Alternation = from alts in SepBy(2, AlternationBranch, Char('|'))
                          select (BasePattern)new AlternationPattern(alts);


            // Groups
            BareGroup = from ps in Many(Choice(Alternation,
                                               Quantifier,
                                               Atom))
                        select new GroupPattern(false, ps);

            ParenGroup = from bare in Between(Char('('),
                                              Char(')'),
                                              BareGroup)
                         select (BasePattern)new GroupPattern(true, bare.Patterns);

            Regex = from bare in BareGroup
                    select new GroupPattern(true, bare.Patterns);
        }


        public static Func<string, string, Parser<char, CharEscapePattern>> CharEscape;
        public static Parser<char, BasePattern> CharEscapeOutsideClass;
        public static Func<bool, bool, bool, Parser<char, CharEscapePattern>> CharEscapeInsideClass;

        public static Parser<char, CharClassPattern> AnyCharClass;
        public static Parser<char, CharClassPattern> NamedCharClass;
        public static Func<bool, bool, Parser<char, CharPattern>> CharRange;
        public static Func<bool, bool, Parser<char, CharPattern>> CharGroupElement;
        public static Func<bool, Parser<char, CharClassPattern>> BareCharGroup;
        public static Parser<char, CharClassPattern> CharClassSubtract;
        public static Parser<char, CharClassPattern> CharGroup;
        public static Parser<char, BasePattern> CharClass;

        public static Parser<char, BasePattern> Anchor;

        public static Parser<char, BasePattern> Atom;

        public static Parser<char, int> NaturalNum;
        public static Parser<char, BasePattern> Quantifier;

        public static Parser<char, BasePattern> AlternationBranch;
        public static Parser<char, BasePattern> Alternation;

        public static Parser<char, GroupPattern> BareGroup;
        public static Parser<char, BasePattern> ParenGroup;
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


        private static Dictionary<char, AnchorType> anchorTypes =
            new Dictionary<char, AnchorType>()
            {
                { '^', AnchorType.StartOfStringOrLine},
                { '$', AnchorType.EndOfStringOrLine },
                { 'A', AnchorType.StartOfString },
                { 'z', AnchorType.EndOfString },
                { 'Z', AnchorType.EndOfStringOrBeforeEndingNewline },
                { 'G', AnchorType.ContiguousMatch },
                { 'b', AnchorType.WordBoundary },
                { 'B', AnchorType.NonWordBoundary },
            };
        private static string bareAnchorKeys = "^$";
        private static string backslashAnchorKeys = anchorTypes.Keys.Cast<char>()
                                                                    .Except(bareAnchorKeys)
                                                                    .AsString();
    }
}
