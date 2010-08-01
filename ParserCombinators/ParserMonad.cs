using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators
{
    public static class ParserMonad
    {
        public static Parser<TToken, TValue> Where<TToken, TValue>(
            this Parser<TToken, TValue> parser,
            Func<TValue, bool> predicate)
        {
            return consList =>
                new ResultSet<TToken, TValue>(
                        parser(consList).Where(
                            result => predicate(result.Value)));
        }

        public static Parser<TToken, TValue2> Select<TToken, TValue, TValue2>(
            this Parser<TToken, TValue> parser,
            Func<TValue, TValue2> selector)
        {
            return consList =>
                new ResultSet<TToken, TValue2>(
                        parser(consList).Select(
                            result => new Result<TToken, TValue2>(selector(result.Value),
                                                                  result.Rest)));
        }

        public static Parser<TToken, TValue2> SelectMany<TToken, TValue, TIntermediate, TValue2>(
            this Parser<TToken, TValue> parser,
            Func<TValue, Parser<TToken, TIntermediate>> selector,
            Func<TValue, TIntermediate, TValue2> projector)
        {
            return consList =>
                new ResultSet<TToken, TValue2>(
                        parser(consList).SelectMany(
                            result => selector(result.Value)(result.Rest),
                            (result, intermResult) => new Result<TToken, TValue2>(projector(result.Value, intermResult.Value),
                                                                                  intermResult.Rest)));
        }
    }
}
