using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.Util;

namespace RegexParser.Patterns
{
    public class QuantifierPattern : BasePattern, IEquatable<QuantifierPattern>
    {
        public QuantifierPattern(BasePattern childPattern, int minOccurrences, int? maxOccurrences, bool isGreedy)
            : base(PatternType.Quantifier)
        {
            if (childPattern == null)
                throw new ArgumentNullException("childPattern.", "Child pattern is null in quantifier pattern.");

            minOccurrences = Math.Max(0, minOccurrences);

            if (maxOccurrences != null)
            {
                maxOccurrences = Math.Max(0, (int)maxOccurrences);

                if (minOccurrences > maxOccurrences)
                    throw new ArgumentException(
                        "Quantifier pattern: the maximum number of occurrences must be greater than or equal to the minimum number.",
                        "maxOccurrences.");
            }

            ChildPattern = childPattern;
            MinOccurrences = minOccurrences;
            MaxOccurrences = maxOccurrences;
            IsGreedy = isGreedy;
        }

        public BasePattern ChildPattern { get; private set; }

        public int MinOccurrences { get; private set; }
        public int? MaxOccurrences { get; private set; }

        public bool IsGreedy { get; private set; }

        /// <summary>
        /// Asserts quantifier is in one of the forms: {n,n}, {0,m}, or {0,} (where n > 0, m > 0)
        /// </summary>
        public void AssertCanonicalForm()
        {
            if (isAssertEnabled &&
                ! ((MinOccurrences == 0 && MaxOccurrences != 0) ||
                   (MinOccurrences  > 0 && MinOccurrences == MaxOccurrences)) )
                throw new ApplicationException(string.Format("Quantifier pattern NOT in canonical form: {{{0},{1}}}.",
                                                             MinOccurrences,
                                                             MaxOccurrences != null ? MaxOccurrences.ToString() : ""));
        }

        public override string ToString()
        {
            return string.Format("Quant {{{0}, Min={1}{2}{3}}}",
                                 ChildPattern.ToString(),
                                 MinOccurrences,
                                 MaxOccurrences != null ? string.Format(", Max={0}", MaxOccurrences) : "",
                                 IsGreedy ? "" : string.Format(", IsGreedy={0}", IsGreedy));
        }

        bool IEquatable<QuantifierPattern>.Equals(QuantifierPattern other)
        {
            return other != null &&
                   this.ChildPattern.Equals(other.ChildPattern) &&
                   this.MinOccurrences == other.MinOccurrences &&
                   this.MaxOccurrences == other.MaxOccurrences &&
                   this.IsGreedy == other.IsGreedy;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(ChildPattern.GetHashCode(), MinOccurrences.GetHashCode(), MaxOccurrences.GetHashCode(),
                                            IsGreedy.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<QuantifierPattern>)this).Equals(obj as QuantifierPattern);
        }
    }
}
