namespace Utility.PrettyPrint
{
    public class PPIncIndent : PPElement
    {
        public PPIncIndent(PPElement child)
            : this("", child) { }

        public PPIncIndent(string tag, PPElement child)
            : base(tag)
        {
            Child = child;
        }

        public PPElement Child { get; private set; }

        public override string Format(int indentLevel, FormatSpecifier formatSpecifier)
        {
            formatSpecifier = formatSpecifier.Recalc(Tag);

            return Child.Format(indentLevel + 1, formatSpecifier);
        }
    }
}
