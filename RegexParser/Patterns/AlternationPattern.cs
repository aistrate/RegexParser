using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    public class AlternationPattern : BasePattern, IEquatable<AlternationPattern>
    {
        public AlternationPattern(IEnumerable<BasePattern> alternatives)
        {
            Alternatives = alternatives.ToArray();
        }

        public AlternationPattern(params BasePattern[] alternatives)
        {
            Alternatives = alternatives;
        }

        public BasePattern[] Alternatives { get; private set; }

        public override string ToString()
        {
            return string.Format("Altern {{{0}}}", Alternatives.Select(p => p.ToString())
                                                               .JoinStrings(" | "));
        }

        bool IEquatable<AlternationPattern>.Equals(AlternationPattern other)
        {
            return other != null && this.Alternatives.SequenceEqual(other.Alternatives);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Alternatives.Select(p => p.GetHashCode()).ToArray());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<AlternationPattern>)this).Equals(obj as AlternationPattern);
        }
    }
}
