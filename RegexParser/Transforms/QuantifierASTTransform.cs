using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// An Abstract Syntax Tree (AST) transform that converts quantifiers into their canonic form:
    ///     - quantifier {n,m} becomes the pair {n,n}, {0,m-n}
    ///     - quantifier {n,}  becomes the pair {n,n}, {0,}
    ///     - quantifiers that are already in target form ({n,n}, {0,m}, or {0,}) remain unchanged
    /// </summary>
    public class QuantifierASTTransform : BaseASTTransform
    {
        public override BasePattern Transform(BasePattern pattern)
        {
            if (pattern.Type == PatternType.Quantifier)
            {
                QuantifierPattern quant = (QuantifierPattern)pattern;

                if (quant.MaxOccurrences == 0)
                    return GroupPattern.Empty;

                BasePattern transformedChild = Transform(quant.ChildPattern);

                if (quant.MinOccurrences == 0 || quant.MinOccurrences == quant.MaxOccurrences)
                    return new QuantifierPattern(transformedChild,
                                                 quant.MinOccurrences,
                                                 quant.MaxOccurrences,
                                                 quant.IsGreedy);

                return new GroupPattern(
                    false,
                    new QuantifierPattern(transformedChild,
                                          quant.MinOccurrences,
                                          quant.MinOccurrences,
                                          quant.IsGreedy),
                    new QuantifierPattern(transformedChild,
                                          0,
                                          quant.MaxOccurrences != null ?
                                                    quant.MaxOccurrences - quant.MinOccurrences :
                                                    null,
                                          quant.IsGreedy));
            }
            else
                return base.Transform(pattern);
        }
    }
}
