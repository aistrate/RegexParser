using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// An Abstract Syntax Tree (AST) transform which helps implementing RegexOptions.
    /// </summary>
    public class RegexOptionsASTTransform : BaseASTTransform
    {
        public RegexOptionsASTTransform(RegexOptionsEx options)
        {
            Options = options;
        }

        public RegexOptionsEx Options { get; private set; }

        public override BasePattern Transform(BasePattern pattern)
        {
            if (Options.Singleline && pattern is AnyCharPattern)
                return new AnyCharPattern(true);
            else
                return base.Transform(pattern);
        }
    }
}
