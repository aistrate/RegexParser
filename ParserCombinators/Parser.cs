using Utility.ConsLists;

namespace ParserCombinators
{
    public delegate Result<TToken, TTree> Parser<TToken, TTree>(IConsList<TToken> consList);
}
