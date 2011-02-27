using Utility.ConsLists;

namespace ParserCombinators
{
    public class Result<TToken, TValue>
    {
        public Result(TValue value, IConsList<TToken> rest)
        {
            Value = value;
            Rest = rest;
        }

        public TValue Value { get; private set; }

        public IConsList<TToken> Rest { get; private set; }
    }
}
