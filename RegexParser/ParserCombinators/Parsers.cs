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
            get { return input => !input.IsEmpty ? new Result<TToken, TToken>(input.Head, input.Tail) : null; }
        }

        public Parser<TToken, TValue> Succeed<TValue>(TValue value)
        {
            return input => new Result<TToken, TValue>(value, input);
        }

        public Parser<TToken, TValue> Fail<TValue>()
        {
            return input => null;
        }

        public Parser<TToken, TValue> EitherOf<TValue>(Parser<TToken, TValue> parser1,
                                                       Parser<TToken, TValue> parser2)
        {
            return input => parser1(input) ?? parser2(input);
        }

        public Parser<TToken, TValue> Choice<TValue>(IEnumerable<Parser<TToken, TValue>> choices)
        {
            return choices.Aggregate<Parser<TToken, TValue>, Parser<TToken, TValue>>(Fail<TValue>(), EitherOf);
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
    }
}
