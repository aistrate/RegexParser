using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public abstract class Parsers<TToken>
    {
        public Parser<TToken, TToken> Token
        {
            get { return consList => !consList.IsEmpty ? new Result<TToken, TToken>(consList.Head, consList.Tail) : null; }
        }

        public Parser<TToken, TValue> Succeed<TValue>(TValue value)
        {
            return consList => new Result<TToken, TValue>(value, consList);
        }

        public Parser<TToken, TValue> Fail<TValue>()
        {
            return consList => null;
        }

        public Parser<TToken, TValue> EitherOf<TValue>(Parser<TToken, TValue> parser1,
                                                       Parser<TToken, TValue> parser2)
        {
            return consList => parser1(consList) ?? parser2(consList);
        }

        public Parser<TToken, TValue> Choice<TValue>(IEnumerable<Parser<TToken, TValue>> choices)
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

        public Parser<TToken, IEnumerable<TValue>> Many<TValue>(Parser<TToken, TValue> parser)
        {
            return EitherOf(Many1(parser), Succeed(Enumerable.Empty<TValue>()));
        }

        public Parser<TToken, IEnumerable<TValue>> Many1<TValue>(Parser<TToken, TValue> parser)
        {
            return from x in parser
                   from xs in Many(parser)
                   select Enumerable.Repeat(x, 1).Concat(xs);
        }

        public Parser<TToken, TValue> Between<TOpen, TClose, TValue>(Parser<TToken, TOpen> open,
                                                                     Parser<TToken, TClose> close,
                                                                     Parser<TToken, TValue> parser)
        {
            return from o in open
                   from x in parser
                   from c in close
                   select x;
        }
    }
}
