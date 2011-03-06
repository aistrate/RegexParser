using System;
using Utility.BaseTypes;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public class CharEscapePattern : CharPattern, IEquatable<CharEscapePattern>
    {
        public CharEscapePattern(char c)
        {
            Value = c;
        }

        public char Value { get; private set; }

        public override bool IsMatch(char c)
        {
            return c == Value;
        }

        public override CharPattern CaseInsensitive
        {
            get
            {
                if (!char.IsLetter(Value))
                    return this;
                else
                    return base.CaseInsensitive;
            }
        }

        public override PPElement ToPrettyPrint()
        {
            return new PPText(Type.ToString(), Value.Show());
        }

        bool IEquatable<CharEscapePattern>.Equals(CharEscapePattern other)
        {
            return other != null && this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharEscapePattern>)this).Equals(obj as CharEscapePattern);
        }
    }
}
