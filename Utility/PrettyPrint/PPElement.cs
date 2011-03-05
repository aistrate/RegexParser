namespace Utility.PrettyPrint
{
    public abstract class PPElement
    {
        public static string FormatAsFlat(PPElement element)
        {
            return element.Format(0, " ", "");
        }

        public static string FormatAsTree(PPElement element, int indentLevel)
        {
            return new PPGroup(
                            new PPMoveToIndent(),
                            element)
                        .Format(indentLevel, "\n", "    ");
        }

        public abstract string Format(int indentLevel, string newlineString, string indentString);
    }
}
