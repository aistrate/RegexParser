using System.Collections.Generic;
using System.Linq;
using Utility.BaseTypes;

namespace Utility.PrettyPrint
{
    public class PPGroup : PPElement
    {
        public PPGroup(IEnumerable<PPElement> children)
            : this("", children)
        {
        }

        public PPGroup(params PPElement[] children)
            : this("", children)
        {
        }

        public PPGroup(PPElement separator, IEnumerable<PPElement> children)
            : this ("", separator, children)
        {
        }

        public PPGroup(string tag, IEnumerable<PPElement> children)
            : this(tag, children.ToArray())
        {
        }

        public PPGroup(string tag, params PPElement[] children)
            : base(tag)
        {
            Children = children;
        }

        public PPGroup(string tag, PPElement separator, IEnumerable<PPElement> children)
            : base(tag)
        {
            Children = children.SelectMany(child => new PPElement[] { separator, child })
                               .Skip(1)
                               .ToArray();
        }

        public PPElement[] Children { get; private set; }

        public override string Format(int indentLevel, FormatSpecifier formatSpecifier)
        {
            formatSpecifier = formatSpecifier.Recalc(Tag);

            return Children.Select(c => c.Format(indentLevel, formatSpecifier))
                           .JoinStrings();
        }
    }
}
