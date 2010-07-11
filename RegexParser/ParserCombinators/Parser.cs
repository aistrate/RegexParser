using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexParser.ParserCombinators.ConsLists;

namespace RegexParser.ParserCombinators
{
    public delegate Result<TToken, TValue> Parser<TToken, TValue>(IConsList<TToken> input);
}
