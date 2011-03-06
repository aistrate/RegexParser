namespace Utility.PrettyPrint
{
    public class PPNewline : PPElement
    {
        public PPNewline()
            : base() { }

        public PPNewline(string tag)
            : base(tag) { }

        public override string Format(int indentLevel, FormatSpecifier formatSpecifier)
        {
            formatSpecifier = formatSpecifier.Recalc(Tag);

            return formatSpecifier.NewlineString +
                   PPMoveToIndent.CalcIndent(indentLevel, formatSpecifier.IndentString);
        }
    }
}
