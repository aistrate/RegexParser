using RegexParser.Patterns;

namespace RegexParser.Transforms
{
    /// <summary>
    /// An Abstract Syntax Tree (AST) transform which helps with implementing RegexOptions.
    /// </summary>
    public class RegexOptionsASTTransform : BaseASTTransform
    {
        public RegexOptionsASTTransform(RegexOptions options)
            : this(new RegexOptionsEx(options))
        {
        }

        public RegexOptionsASTTransform(RegexOptionsEx options)
        {
            Options = options;
        }

        public RegexOptionsEx Options { get; private set; }

        public override BasePattern Transform(BasePattern pattern)
        {
            BasePattern transformed = pattern;

            if (pattern.Type == PatternType.Char)
            {
                if (Options.Singleline && transformed is AnyCharPattern)
                    transformed = new AnyCharPattern(true);

                if (Options.IgnoreCase)
                    transformed = ((CharPattern)transformed).CaseInsensitive;
            }
            else if (pattern.Type == PatternType.Anchor)
            {
                AnchorPattern anchor = (AnchorPattern)pattern;

                if (anchor.AnchorType == AnchorType.StartOfStringOrLine)
                    transformed = new AnchorPattern(Options.Multiline ? AnchorType.StartOfLine : AnchorType.StartOfString);
                else if (anchor.AnchorType == AnchorType.EndOfStringOrLine)
                    transformed = new AnchorPattern(Options.Multiline ? AnchorType.EndOfLine : AnchorType.EndOfString);
            }

            if (transformed != pattern)
                return transformed;
            else
                return base.Transform(pattern);
        }
    }
}
