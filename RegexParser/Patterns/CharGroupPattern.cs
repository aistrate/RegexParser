using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    public class CharGroupPattern : BasePattern, IEquatable<CharGroupPattern>
    {
        public CharGroupPattern(bool isPositive, IEnumerable<char> characters)
        {
            IsPositive = isPositive;
            Characters = characters.GroupBy(c => c)
                                   .Select(g => g.First())
                                   .ToArray();
        }

        public bool IsPositive { get; private set; }
        public char[] Characters { get; private set; }

        public override string ToString()
        {
            return string.Format("CharGroup {{{0}\"{1}\"}}", IsPositive ? "" : "^", new string(Characters));
        }

        bool IEquatable<CharGroupPattern>.Equals(CharGroupPattern other)
        {
            return other != null && this.IsPositive == other.IsPositive && this.Characters == other.Characters;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(IsPositive.GetHashCode(), Characters.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<CharGroupPattern>)this).Equals(obj as CharGroupPattern);
        }
    }
}
