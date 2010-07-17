using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.ConsLists;

namespace ParserCombinators
{
    public class Parsers<TToken>
    {
        public static Parser<TToken, TToken> Token
        {
            get { return consList => !consList.IsEmpty ? new Result<TToken, TToken>(consList.Head, consList.Tail) : null; }
        }

        public static Parser<TToken, TValue> Succeed<TValue>(TValue value)
        {
            return consList => new Result<TToken, TValue>(value, consList);
        }

        public static Parser<TToken, TValue> Fail<TValue>()
        {
            return consList => null;
        }

        public static Parser<TToken, TValue> Either<TValue>(Parser<TToken, TValue> parser1, Parser<TToken, TValue> parser2)
        {
            return consList => parser1(consList) ?? parser2(consList);
        }

        public static Parser<TToken, TValue> Choice<TValue>(IEnumerable<Parser<TToken, TValue>> choices)
        {
            return consList =>
            {
                foreach (var parser in choices)
                {
                    var value = parser(consList);
                    if (value != null)
                        return value;
                }

                return null;
            };
        }

        public static Parser<TToken, TValue> Option<TValue>(TValue defaultValue, Parser<TToken, TValue> parser)
        {
            return Either(parser, Succeed(defaultValue));
        }

        public static Parser<TToken, IEnumerable<TValue>> Many<TValue>(Parser<TToken, TValue> parser)
        {
            return consList =>
            {
                List<TValue> values = new List<TValue>();
                Result<TToken, TValue> result;

                do
                {
                    result = parser(consList);

                    if (result != null)
                    {
                        values.Add(result.Value);
                        consList = result.Rest;
                    }
                }
                while (result != null);

                return new Result<TToken, IEnumerable<TValue>>(values, consList);
            };
        }

        public static Parser<TToken, IEnumerable<TValue>> Many1<TValue>(Parser<TToken, TValue> parser)
        {
            return from x in parser
                   from xs in Many(parser)
                   select new[] { x }.Concat(xs);
        }

        public static Parser<TToken, TValue> Between<TOpen, TClose, TValue>(Parser<TToken, TOpen> open,
                                                                            Parser<TToken, TClose> close,
                                                                            Parser<TToken, TValue> parser)
        {
            return from o in open
                   from x in parser
                   from c in close
                   select x;
        }

        public static Parser<TToken, UnitType> NotFollowedBy<TValue>(Parser<TToken, TValue> parser)
        {
            return consList => parser(consList) != null ? null : new Result<TToken, UnitType>(UnitType.Unit, consList);
        }

        public static Parser<TToken, UnitType> Eof
        {
            get { return NotFollowedBy(Token); }
        }

        public static Parser<TToken, TValue> Lazy<TValue>(Func<Parser<TToken, TValue>> thunk)
        {
            return consList => thunk()(consList);
        }

        public static IEnumerable<T> LazySeq<T>(params Func<T>[] thunks)
        {
            return thunks.Select(t => t());
        }
    }
}
