using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Pattern
{
    public class CharPattern : BasePattern, IEquatable<CharPattern>
    {
        public CharPattern(char c)
        {
            Value = c;
        }

        public char Value { get; private set; }

        public override string ToString()
        {
            return string.Format("Char {{'{0}'}}", Value);
        }

        bool IEquatable<CharPattern>.Equals(CharPattern other)
        {
            return other != null && this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharPattern>)this).Equals(obj as CharPattern);
        }
    }
}
