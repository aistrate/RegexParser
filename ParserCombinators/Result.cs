using Utility.ConsLists;

namespace ParserCombinators
{
    public class Result<TToken, TTree>
    {
        public Result(TTree tree, IConsList<TToken> rest)
        {
            Tree = tree;
            Rest = rest;
        }

        public TTree Tree { get; private set; }

        public IConsList<TToken> Rest { get; private set; }
    }
}
