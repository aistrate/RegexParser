using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RegexParser
{
    public class CaptureCollection2 : ReadOnlyCollection<Capture2>
    {
        public CaptureCollection2(IEnumerable<Capture2> captures)
            : base(captures.ToList())
        {
        }

        public CaptureCollection2(IList<Capture2> captures)
            : base(captures)
        {
        }
    }
}
