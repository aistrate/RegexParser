using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.Util
{
    /// <summary>
    /// The unit datatype () from Haskell, with only one member, ().
    /// </summary>
    public struct UnitType
    {
        public static UnitType Unit { get { return new UnitType(); } }
    }
}
