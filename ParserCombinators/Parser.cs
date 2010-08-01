using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.ConsLists;

namespace ParserCombinators
{
    public delegate ResultSet<TToken, TValue> Parser<TToken, TValue>(IConsList<TToken> consList);
}
