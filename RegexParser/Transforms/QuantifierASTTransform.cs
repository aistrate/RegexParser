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

                if (quant.MaxOccurrences == 0)
                    return GroupPattern.Empty;

                BasePattern transformedChild = Transform(quant.ChildPattern);

                if (quant.MinOccurrences == 0)
                    return new QuantifierPattern(transformedChild,
                                                 0,
                                                 quant.MaxOccurrences,
                                                 quant.IsGreedy);
                else if (quant.MinOccurrences == quant.MaxOccurrences)
                    return createQuantifier(transformedChild,
                                            quant.MinOccurrences,
                                            quant.IsGreedy);
                else
                    return new GroupPattern(
                        false,
                        createQuantifier(transformedChild,
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

        private BasePattern createQuantifier(BasePattern childPattern, int occurrences, bool isGreedy)
        {
            if (occurrences == 1)
                return childPattern;
            else
                return new QuantifierPattern(childPattern, occurrences, occurrences, isGreedy);
        }
    }
}
