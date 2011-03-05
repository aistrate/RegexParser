using System.Collections.Generic;
using System.Linq;
using Utility.BaseTypes;

namespace Utility.PrettyPrint
{
    public class PPGroup : PPElement
    {
        public PPGroup(IEnumerable<PPElement> children)
            : this(children.ToArray())
        {
        }

        public PPGroup(params PPElement[] children)
        {
            Children = children;
        }

        public PPGroup(PPElement separator, IEnumerable<PPElement> children)
        {
            Children = children.SelectMany(child => new PPElement[] { separator, child })
                               .Skip(1)
                               .ToArray();
        }

        public PPElement[] Children { get; private set; }

        public override string Format(int indentLevel, string newlineString, string indentString)
        {
            return Children.Select(c => c.Format(indentLevel, newlineString, indentString))
                           .JoinStrings();
        }
    }
}
