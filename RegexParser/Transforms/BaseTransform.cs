using System.Linq;
using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// Class representing a pattern tree transform.
    /// </summary>
    public abstract class BaseTransform
    {
        public virtual BasePattern RunTransform(BasePattern pattern)
        {
            // The identity transform

            if (pattern is GroupPattern)
                return new GroupPattern(((GroupPattern)pattern).Patterns
                                                               .Select(a => RunTransform(a)));

            else if (pattern is QuantifierPattern)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                return new QuantifierPattern(RunTransform(quant.ChildPattern),
                                             quant.MinOccurrences, quant.MaxOccurrences, quant.IsGreedy);
            }

            else if (pattern is AlternationPattern)
                return new AlternationPattern(((AlternationPattern)pattern).Alternatives
                                                                           .Select(a => RunTransform(a)));

            else
                return pattern;
        }
    }
}
