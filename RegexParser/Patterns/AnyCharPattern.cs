using System;

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

        public override string ToString()
        {
            return string.Format("AnyChar{0}Newline", AcceptNewline ? "Incl" : "Excl");
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
