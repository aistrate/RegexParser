namespace Utility.PrettyPrint
{
    public class PPNewline : PPElement
    {
        public override string Format(int indentLevel, string newlineString, string indentString)
        {
            return newlineString +
                   PPMoveToIndent.CalcIndent(indentLevel, indentString);
        }
    }
}
