using System;
using System.Linq;
using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// An Abstract Syntax Tree (AST) transform that converts quantifiers to their canonic form:
    ///     - quantifier {n,m} becomes pair {n,n}, {0,m-n}
    ///     - quantifier {n,}  becomes pair {n,n}, {0,}
    ///     - quantifiers already in target form ({n,n}, {0,m}, or {0,}) remain unchanged
    /// </summary>
    public class QuantifierASTTransform : BaseASTTransform
    {
        public override BasePattern Transform(BasePattern pattern)
        {
            if (pattern.Type == PatternType.Quantifier)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;
                BasePattern transformedChild = Transform(quant.ChildPattern);

                if (IsEmpty(transformedChild))
                    return GroupPattern.Empty;

                BasePattern[] newPatterns = new[]
                    {
                        new QuantifierPattern(transformedChild,
                                              quant.MinOccurrences,
                                              quant.MinOccurrences,
                                              quant.IsGreedy),
                        new QuantifierPattern(transformedChild,
                                              0,
                                              quant.MaxOccurrences != null ? quant.MaxOccurrences - quant.MinOccurrences : null,
                                              quant.IsGreedy),
                    }
                    .Select(q => reduceQuantifier(q))
                    .Where(IsNotEmpty)
                    .ToArray();

                if (newPatterns.Length == 1)
                    return newPatterns[0];
                else
                    return new GroupPattern(false, newPatterns);
            }
            else
                return base.Transform(pattern);
        }

        private BasePattern reduceQuantifier(QuantifierPattern quant)
        {
            if (quant.MaxOccurrences == 0)
                return GroupPattern.Empty;
            else if (quant.MinOccurrences == 1 && quant.MaxOccurrences == 1)
                return quant.ChildPattern;
            else
                return quant;
        }
    }
}
