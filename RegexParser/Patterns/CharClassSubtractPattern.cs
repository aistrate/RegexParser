using System;
using Utility.General;

namespace RegexParser.Patterns
{
    public class CharClassSubtractPattern : CharClassPattern, IEquatable<CharClassSubtractPattern>
    {
        public CharClassSubtractPattern(CharClassPattern baseClass, CharClassPattern excludedClass)
        {
            BaseClass = baseClass;
            ExcludedClass = excludedClass;
        }

        public CharClassPattern BaseClass { get; private set; }
        public CharClassPattern ExcludedClass { get; private set; }

        public override bool IsMatch(char c)
        {
            return BaseClass.IsMatch(c) && !ExcludedClass.IsMatch(c);
        }

        public override string ToString()
        {
            return string.Format("CharClassSubtr {{ {0}, Excluded={1} }}", BaseClass.ToString(), ExcludedClass.ToString());
        }

        bool IEquatable<CharClassSubtractPattern>.Equals(CharClassSubtractPattern other)
        {
            return other != null && this.BaseClass == other.BaseClass && this.ExcludedClass == other.ExcludedClass;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(BaseClass.GetHashCode(), ExcludedClass.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharClassSubtractPattern>)this).Equals(obj as CharClassSubtractPattern);
        }
    }
}
