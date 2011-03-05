namespace Utility.PrettyPrint
{
    public class PPIncIndent : PPElement
    {
        public PPIncIndent(PPElement child)
        {
            Child = child;
        }

        public PPElement Child { get; private set; }

        public override string Format(int indentLevel, string newlineString, string indentString)
        {
            return Child.Format(indentLevel + 1, newlineString, indentString);
        }
    }
}
