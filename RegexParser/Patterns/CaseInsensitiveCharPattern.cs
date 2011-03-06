using System;
using System.Linq;
using Utility.PrettyPrint;
using Utility.BaseTypes;

namespace RegexParser.Patterns
{
    public class CaseInsensitiveCharPattern : CharClassPattern, IEquatable<CaseInsensitiveCharPattern>
    {
        public CaseInsensitiveCharPattern(CharPattern childPattern)
        {
            if (childPattern == null)
                throw new ArgumentNullException("childPattern.", "Child pattern is null in case-insensitive char pattern.");

            ChildPattern = childPattern;
        }

        public CharPattern ChildPattern { get; private set; }

        public override bool IsMatch(char c)
        {
            if (char.IsLetter(c))
                return ChildPattern.IsMatch(char.ToLower(c)) ||
                       ChildPattern.IsMatch(char.ToUpper(c));
            else
                return ChildPattern.IsMatch(c);
        }

        public override CharPattern CaseInsensitive { get { return this; } }

        public override PPElement ToPrettyPrint()
        {
            return new PPGroup(
                            Type.ToString(),
                            new PPText("CaseInsensChar"),
                            PPGroupWithDelimiters(new PPElement[] { ChildPattern.ToPrettyPrint() }));
        }

        bool IEquatable<CaseInsensitiveCharPattern>.Equals(CaseInsensitiveCharPattern other)
        {
            return other != null &&
                   this.ChildPattern.Equals(other.ChildPattern);
        }

        public override int GetHashCode()
        {
            return ChildPattern.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CaseInsensitiveCharPattern>)this).Equals(obj as CaseInsensitiveCharPattern);
        }
    }
}
