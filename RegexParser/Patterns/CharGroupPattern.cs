using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class CharGroupPattern : CharClassPattern, IEquatable<CharGroupPattern>
    {
        //public CharGroupPattern(bool isPositive, IEnumerable<char> charSet)
        //    : this(isPositive, charSet, null) { }

        //public CharGroupPattern(bool isPositive, IEnumerable<CharRange> charRanges)
        //    : this(isPositive, null, charRanges) { }

        //public CharGroupPattern(bool isPositive, IEnumerable<char> charSet, IEnumerable<CharRange> charRanges)
        //{
        //    IsPositive = isPositive;

        //    if (charSet != null)
        //        CharSet = new string(charSet.Distinct().ToArray());

        //    if (charRanges != null)
        //        CharRanges = charRanges.Distinct().ToArray();
        //}

        //public CharGroupPattern(bool isPositive, IEnumerable<CharPattern> charPatterns)
        //    : this(isPositive, extractCharSet(charPatterns), extractCharRanges(charPatterns)) { }

        public CharGroupPattern(bool isPositive, string charSet)
            : this(isPositive, charSet, null) { }

        public CharGroupPattern(bool isPositive, string charSet, IEnumerable<CharPattern> childPatterns)
            : this(isPositive, charSet.Select(c => (CharPattern)new CharEscapePattern(c))
                                      .Concat(childPatterns ?? new CharPattern[] { }))
        {
        }

        public CharGroupPattern(bool isPositive, IEnumerable<CharPattern> childPatterns)
        {
            IsPositive = isPositive;

            if (childPatterns != null)
                ChildPatterns = childPatterns.Distinct().ToArray();
        }

        private CharGroupPattern(bool isPositive, CharGroupPattern original)
        {
            IsPositive = isPositive;
            ChildPatterns = original.ChildPatterns;
            //CharSet = original.CharSet;
            //CharRanges = original.CharRanges;
        }

        //private static IEnumerable<char> extractCharSet(IEnumerable<CharPattern> charPatterns)
        //{
        //    return charPatterns.OfType<CharEscapePattern>()
        //                       .Select(c => c.Value)
        //                       .Concat(charPatterns.OfType<CharGroupPattern>()
        //                                           .SelectMany(cls => cls.CharSet));
        //}

        //private static IEnumerable<CharRange> extractCharRanges(IEnumerable<CharPattern> charPatterns)
        //{
        //    return charPatterns.OfType<CharGroupPattern>()
        //                       .SelectMany(cls => cls.CharRanges);
        //}

        public readonly bool IsPositive;
        public readonly CharPattern[] ChildPatterns = new CharPattern[] { };

        //public readonly string CharSet = "";
        //public readonly CharRangePattern[] CharRanges = new CharRangePattern[] { };

        public override bool IsMatch(char c)
        {
            return !IsPositive ^ isPositiveMatch(c);

            //return !IsPositive ^ ChildPatterns.Any(p => p.IsMatch(c));
            //return !IsPositive ^ Array.FindIndex(ChildPatterns, p => p.IsMatch(c)) >= 0;
        }

        private bool isPositiveMatch(char c)
        {
            foreach (var childPattern in ChildPatterns)
                if (childPattern.IsMatch(c))
                    return true;

            return false;
        }

        //private bool isPositiveMatch(char c)
        //{
        //    return ChildPatterns.Any(p => p.IsMatch(c));

        //    //foreach (var charRange in CharRanges)
        //    //    if (charRange.From <= c && c <= charRange.To)
        //    //        return true;

        //    //return CharSet.IndexOf(c) >= 0;
        //}

        public override string ToString()
        {
            return string.Format("CharGroup {{{0}{1}}}",
                                 IsPositive ? "" : "^ ",
                                 (string.Join(", ", ChildPatterns.Select(p => p.ToString()).ToArray())));

            //return string.Format("CharGroup {{{0}{1}}}",
            //                     IsPositive ? "" : "^ ",
            //                     (string.Join("", CharRanges.Select(r => r.ToString()).ToArray()) + CharSet).Show());
        }

        bool IEquatable<CharGroupPattern>.Equals(CharGroupPattern other)
        {
            return other != null &&
                   this.IsPositive == other.IsPositive &&
                   this.ChildPatterns.SequenceEqual(other.ChildPatterns);

                   //this.CharSet.SequenceEqual(other.CharSet) &&
                   //this.CharRanges.SequenceEqual(other.CharRanges);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(IsPositive.GetHashCode(),
                                            HashCodeCombiner.Combine(ChildPatterns.Select(p => p.GetHashCode()).ToArray()));

            //return HashCodeCombiner.Combine(IsPositive.GetHashCode(), CharSet.GetHashCode(),
            //                                HashCodeCombiner.Combine(CharRanges.Select(r => r.GetHashCode()).ToArray()));
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharGroupPattern>)this).Equals(obj as CharGroupPattern);
        }


        public CharGroupPattern Negated { get { return new CharGroupPattern(!IsPositive, this); } }

        public static readonly CharGroupPattern AnyChar = new CharGroupPattern(false, "\n");

        public static readonly CharGroupPattern WhitespaceChar = new CharGroupPattern(true, " \n\r\t\f\v");

        public static readonly CharGroupPattern WordChar = new CharGroupPattern(true, "_", new[]
                                                                                {
                                                                                    new CharRangePattern('a', 'z'),
                                                                                    new CharRangePattern('A', 'Z'),
                                                                                    new CharRangePattern('0', '9')
                                                                                });

        public static readonly CharGroupPattern DigitChar = new CharGroupPattern(true, new[]
                                                                                 {
                                                                                     new CharRangePattern('0', '9')
                                                                                 });
    }
}
