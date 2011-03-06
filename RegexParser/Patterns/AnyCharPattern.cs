using System;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public class AnyCharPattern : CharClassPattern, IEquatable<AnyCharPattern>
    {
        public AnyCharPattern(bool acceptNewline)
        {
            AcceptNewline = acceptNewline;
        }

        public readonly bool AcceptNewline;

        public override bool IsMatch(char c)
        {
            return AcceptNewline || c != '\n';
        }

        public override CharPattern CaseInsensitive { get { return this; } }

        public override PPElement ToPrettyPrint()
        {
            return new PPText(Type.ToString(), string.Format("AnyChar (AcceptNL={0})", AcceptNewline));
        }

        bool IEquatable<AnyCharPattern>.Equals(AnyCharPattern other)
        {
            return other != null &&
                   this.AcceptNewline == other.AcceptNewline;
        }

        public override int GetHashCode()
        {
            return AcceptNewline.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<AnyCharPattern>)this).Equals(obj as AnyCharPattern);
        }
    }
}
