using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RegexParser
{
    public class GroupCollection2 : ReadOnlyCollection<Group2>
    {
        public GroupCollection2(IEnumerable<Group2> groups)
            : base(groups.ToList())
        {
        }

        public GroupCollection2(IList<Group2> groups)
            : base(groups)
        {
        }
    }
}
