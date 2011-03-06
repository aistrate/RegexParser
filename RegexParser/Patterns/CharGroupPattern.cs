using System;
using System.Collections.Generic;
using System.Linq;
using Utility.BaseTypes;
using Utility.General;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class CharGroupPattern : CharClassPattern, IEquatable<CharGroupPattern>
    {
        public CharGroupPattern(bool isPositive, string charSet)
            : this(isPositive, charSet, null) { }

        public CharGroupPattern(bool isPositive, IEnumerable<CharPattern> childPatterns)
            : this(isPositive, null, childPatterns) { }

        public CharGroupPattern(bool isPositive, string charSet, IEnumerable<CharPattern> childPatterns)
        {
            IsPositive = isPositive;

            string charSet2 = "";

            if (childPatterns != null)
            {
                var charEscapes = childPatterns.OfType<CharEscapePattern>();

                charSet2 = charEscapes.Select(c => c.Value)
                                      .AsString();

                ChildPatterns = childPatterns.Except(charEscapes.Cast<CharPattern>())
                                             .Distinct()
                                             .ToArray();
            }

            CharSet = ((charSet ?? "") + charSet2).Distinct()
                                                  .AsString();
        }

        private CharGroupPattern(bool isPositive, CharGroupPattern original)
        {
            IsPositive = isPositive;
            CharSet = original.CharSet;
            ChildPatterns = original.ChildPatterns;
        }

        public readonly bool IsPositive;
        public readonly string CharSet = "";
        public readonly CharPattern[] ChildPatterns = new CharPattern[] { };

        public override bool IsMatch(char c)
        {
            return !IsPositive ^ isPositiveMatch(c);
        }

        private bool isPositiveMatch(char c)
        {
            foreach (var childPattern in ChildPatterns)
                if (childPattern.IsMatch(c))
                    return true;

            return CharSet.IndexOf(c) >= 0;
        }

        public override CharPattern CaseInsensitive
        {
            get
            {
                if (predefined.Contains(this) ||
                    predefined.Contains(this.Negated))
                    return this;
                else
                    return base.CaseInsensitive;
            }
        }

        public override PPElement ToPrettyPrint()
        {
            var childPPElements = Enumerable.Repeat<PPElement>(new PPText(string.Format("[{0}]", CharSet.Show())),
                                                               CharSet != "" || ChildPatterns.Length == 0 ? 1 : 0)
                                            .Concat(ChildPatterns.Select(p => p.ToPrettyPrint()));

            return new PPGroup(
                            Type.ToString(),
                            new PPText(string.Format("CharClass{0}", IsPositive ? "" : " (Neg)")),
                            PPGroupWithDelimiters(childPPElements));

        }

        bool IEquatable<CharGroupPattern>.Equals(CharGroupPattern other)
        {
            return other != null &&
                   this.IsPositive == other.IsPositive &&
                   this.CharSet.SequenceEqual(other.CharSet) &&
                   this.ChildPatterns.SequenceEqual(other.ChildPatterns);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(IsPositive.GetHashCode(), CharSet.GetHashCode(),
                                            HashCodeCombiner.Combine(ChildPatterns.Select(p => p.GetHashCode()).ToArray()));
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharGroupPattern>)this).Equals(obj as CharGroupPattern);
        }


        public CharGroupPattern Negated { get { return new CharGroupPattern(!IsPositive, this); } }

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

        private static readonly CharGroupPattern[] predefined = new []
        {
            WhitespaceChar,
            WordChar,
            DigitChar,
        };
    }
}
