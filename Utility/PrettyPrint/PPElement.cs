namespace Utility.PrettyPrint
{
    public abstract class PPElement
    {
        public PPElement()
            : this ("") { }

        public PPElement(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; private set; }

        public abstract string Format(int indentLevel, FormatSpecifier formatSpecifier);


        public string FormatAsFlat()
        {
            return FormatAsTree(0, StaticFormatSpecifier.Collapsed);
        }

        public string FormatAsTree(int indentLevel)
        {
            return FormatAsTree(indentLevel, StaticFormatSpecifier.Expanded);
        }

        public string FormatAsTree(int indentLevel, FormatSpecifier formatSpecifier)
        {
            return new PPGroup(
                            new PPMoveToIndent(),
                            this)
                        .Format(indentLevel, formatSpecifier);
        }
    }
}
