using System.Linq;
using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// Class representing an Abstract Syntax Tree (AST) transform.
    /// </summary>
    public abstract class BaseASTTransform
    {
        public virtual BasePattern Transform(BasePattern pattern)
        {
            // The Identity transform

            switch (pattern.Type)
            {
                case PatternType.Group:
                    BasePattern[] newChildren = ((GroupPattern)pattern).Patterns
                                                                       .Select(p => Transform(p))
                                                                       .Where(IsNotEmpty)
                                                                       .ToArray();

                    return CreateGroupOrSingleton(newChildren);


                case PatternType.Quantifier:
                    QuantifierPattern quant = (QuantifierPattern)pattern;
                    BasePattern newChild = Transform(quant.ChildPattern);

                    if (IsNotEmpty(newChild))
                        return new QuantifierPattern(newChild, quant.MinOccurrences, quant.MaxOccurrences, quant.IsGreedy);
                    else
                        return GroupPattern.Empty;


                case PatternType.Alternation:
                    return new AlternationPattern(((AlternationPattern)pattern).Alternatives
                                                                               .Select(a => Transform(a)));


                default:
                    return pattern;
            }
        }

        protected BasePattern CreateGroupOrSingleton(BasePattern[] children)
        {
            if (children.Length == 1)
                return children[0];
            else
                return new GroupPattern(children);
        }

        protected bool IsNotEmpty(BasePattern pattern)
        {
            return !(pattern.Type == PatternType.Group && pattern.Equals(GroupPattern.Empty));
        }
    }
}
