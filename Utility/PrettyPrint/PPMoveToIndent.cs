using System.Linq;
using Utility.BaseTypes;

namespace Utility.PrettyPrint
{
    public class PPMoveToIndent : PPElement
    {
        public override string Format(int indentLevel, string newlineString, string indentString)
        {
            return CalcIndent(indentLevel, indentString);
        }

        internal static string CalcIndent(int indentLevel, string indentString)
        {
            return Enumerable.Repeat(indentString, indentLevel)
                             .JoinStrings();
        }
    }
}
