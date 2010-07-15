using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    public class CharClassPattern : BasePattern, IEquatable<CharClassPattern>
    {
        public CharClassPattern(bool isPositive, IEnumerable<char> characterSet)
        {
            IsPositive = isPositive;
            CharacterSet = characterSet.GroupBy(c => c)
                                       .Select(g => g.First())
                                       .ToArray();
        }

        public bool IsPositive { get; private set; }
        public char[] CharacterSet { get; private set; }

        public override string ToString()
        {
            return string.Format("CharClass {{{0}\"{1}\"}}", IsPositive ? "" : "^", new string(CharacterSet));
        }

        bool IEquatable<CharClassPattern>.Equals(CharClassPattern other)
        {
            return other != null && this.IsPositive == other.IsPositive && this.CharacterSet == other.CharacterSet;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(IsPositive.GetHashCode(), CharacterSet.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharClassPattern>)this).Equals(obj as CharClassPattern);
        }
    }
}
