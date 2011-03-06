using System.Linq;
using Utility.BaseTypes;

namespace Utility.PrettyPrint
{
    public class PPMoveToIndent : PPElement
    {
        public PPMoveToIndent()
            : base() { }

        public PPMoveToIndent(string tag)
            : base(tag) { }

        public override string Format(int indentLevel, FormatSpecifier formatSpecifier)
        {
            formatSpecifier = formatSpecifier.Recalc(Tag);

            return CalcIndent(indentLevel, formatSpecifier.IndentString);
        }

        internal static string CalcIndent(int indentLevel, string indentString)
        {
            return Enumerable.Repeat(indentString, indentLevel)
                             .JoinStrings();
        }
    }
}
