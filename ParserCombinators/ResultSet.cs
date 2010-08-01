using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.ConsLists;
using System.Collections;

namespace ParserCombinators
{
    public class ResultSet<TToken, TValue> : IEnumerable<Result<TToken, TValue>>
    {
        public ResultSet(IEnumerable<Result<TToken, TValue>> collection)
        {
            this.collection = collection;
        }

        private ResultSet(Result<TToken, TValue> singleResult)
            : this(new[] { singleResult })
        {
        }

        private ResultSet(TValue value, IConsList<TToken> rest)
            : this(new Result<TToken, TValue>(value, rest))
        {
        }

        private IEnumerable<Result<TToken, TValue>> collection;

        IEnumerator<Result<TToken, TValue>> IEnumerable<Result<TToken, TValue>>.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }


        public static ResultSet<TToken, TValue> Empty()
        {
            return new ResultSet<TToken, TValue>(Enumerable.Empty<Result<TToken, TValue>>());
        }

        public static ResultSet<TToken, TValue> SingleResult(Result<TToken, TValue> result)
        {
            return new ResultSet<TToken, TValue>(result);
        }

        public static ResultSet<TToken, TValue> SingleResult(TValue value, IConsList<TToken> rest)
        {
            return new ResultSet<TToken, TValue>(value, rest);
        }
    }
}
