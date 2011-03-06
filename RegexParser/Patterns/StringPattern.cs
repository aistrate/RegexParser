using System;
using Utility.BaseTypes;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public class StringPattern : BasePattern, IEquatable<StringPattern>
    {
        public StringPattern(string s)
            : base(PatternType.String, s.Length)
        {
            Value = s;
        }

        public string Value { get; private set; }

        public override PPElement ToPrettyPrint()
        {
            return new PPText(Type.ToString(), Value.Show());
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
