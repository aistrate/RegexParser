using System;
using System.Collections.Generic;
using System.Linq;
using Utility.BaseTypes;
using Utility.General;
using Utility.PrettyPrint;

namespace RegexParser.Patterns
{
    public class AlternationPattern : BasePattern, IEquatable<AlternationPattern>
    {
        public AlternationPattern(IEnumerable<BasePattern> alternatives)
            : this(alternatives.ToArray())
        {
        }

        public AlternationPattern(params BasePattern[] alternatives)
            : base(PatternType.Alternation, alternatives.Min(a => a.MinCharLength))
        {
            Alternatives = alternatives;
        }

        public BasePattern[] Alternatives { get; private set; }

        public override PPElement ToPrettyPrint()
        {
            return new PPGroup(
                            new PPText("Altern"),
                            new PPNewline(),
                            new PPText("{"),
                            new PPIncIndent(
                                new PPGroup(
                                    new PPGroup(
                                        new PPNewline(),
                                        new PPText("Or")),
                                    Alternatives.Select<BasePattern, PPElement>(a =>
                                        new PPGroup(
                                            new PPNewline(),
                                            a.ToPrettyPrint())))),
                            new PPNewline(),
                            new PPText("}"));
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
