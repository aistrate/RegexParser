using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators
{
    public abstract class Parsers<TInput>
    {
        public Parser<TInput, TValue> Succeed<TValue>(TValue value)
        {
            return input => new Result<TInput, TValue>(value, input);
        }

        public Parser<TInput, TValue> EitherOf<TValue>(Parser<TInput, TValue> parser1,
                                                       Parser<TInput, TValue> parser2)
        {
            return input => parser1(input) ?? parser2(input);
        }

        // TODO: method Choice()

        // TODO: change TValue[] into IEnumerable<TValue>
        public Parser<TInput, TValue[]> Many<TValue>(Parser<TInput, TValue> parser)
        {
            return EitherOf(Many1(parser), Succeed(new TValue[0]));
        }

        public Parser<TInput, TValue[]> Many1<TValue>(Parser<TInput, TValue> parser)
        {
            return from x in parser
                   from xs in Many(parser)
                   select (new[] { x }).Concat(xs).ToArray();
        }
    }
}
