using Utility.ConsLists;

namespace ParserCombinators
{
    public delegate Result<TToken, TValue> Parser<TToken, TValue>(IConsList<TToken> consList);
}
