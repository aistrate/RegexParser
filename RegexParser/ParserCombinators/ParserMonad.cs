using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public static class ParserMonad
    {
        public static Parser<TToken, TValue> Where<TToken, TValue>(this Parser<TToken, TValue> parser,
                                                                   Func<TValue, bool> predicate)
        {
            return input =>
            {
                var result = parser(input);
                
                if (result != null && predicate(result.Value))
                    return result;
                else
                    return null;
            };
        }

        public static Parser<TToken, TValue2> Select<TToken, TValue, TValue2>(this Parser<TToken, TValue> parser,
                                                                              Func<TValue, TValue2> selector)
        {
            return input =>
            {
                var result = parser(input);
                
                if (result != null)
                    return new Result<TToken, TValue2>(selector(result.Value), result.Rest);
                else
                    return null;
            };
        }

        public static Parser<TToken, TValue2> SelectMany<TToken, TValue, TIntermediate, TValue2>(this Parser<TToken, TValue> parser,
                                                                                                 Func<TValue, Parser<TToken, TIntermediate>> selector,
                                                                                                 Func<TValue, TIntermediate, TValue2> projector)
        {
            return input =>
            {
                var result = parser(input);

                if (result != null)
                {
                    var result2 = selector(result.Value)(result.Rest);
                    
                    if (result2 != null)
                        return new Result<TToken, TValue2>(projector(result.Value, result2.Value), result2.Rest);
                }
                
                return null;
            };
        }
    }
}
