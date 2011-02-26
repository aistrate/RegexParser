using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;

namespace RegexParser.Patterns
{
    public class StringPattern : BasePattern, IEquatable<StringPattern>
    {
        public StringPattern(string s)
            : base(PatternType.String)
        {
            Value = s;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.Show();
        }

        bool IEquatable<StringPattern>.Equals(StringPattern other)
        {
            return other != null && this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<StringPattern>)this).Equals(obj as StringPattern);
        }
    }
}
