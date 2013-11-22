using System;
using System.Collections.Generic;
using System.Linq;
using Utility.BaseTypes;

namespace ParserCombinators
{
    public class Parsers<TToken>
    {
        // TODO: 'Try' parser (see [Leijen 2001], page 7), to improve performance (?)

        public static Parser<TToken, TToken> Token
        {
            get { return consList => !consList.IsEmpty ? new Result<TToken, TToken>(consList.Head, consList.Tail) : null; }
        }

        public static Parser<TToken, TTree> Succeed<TTree>(TTree tree)
        {
            return consList => new Result<TToken, TTree>(tree, consList);
        }

        public static Parser<TToken, TTree> Fail<TTree>()
        {
            return consList => null;
        }

        public static Parser<TToken, TTree> Choice<TTree>(params Parser<TToken, TTree>[] choices)
        {
            return consList =>
            {
                foreach (var parser in choices)
                {
                    var result = parser(consList);
                    if (result != null)
                        return result;
                }

                return null;
            };
        }

        public static Parser<TToken, TTree> Option<TTree>(TTree defaultTree, Parser<TToken, TTree> parser)
        {
            return Choice(parser, Succeed(defaultTree));
        }

        public static Parser<TToken, TTree?> OptionNullable<TTree>(Parser<TToken, TTree> parser)
            where TTree : struct
        {
            return Option(null,
                          from x in parser select (TTree?)x);
        }

        public static Parser<TToken, IEnumerable<TTree>> Many<TTree>(Parser<TToken, TTree> parser)
        {
            return Count(0, null, parser);
        }

        public static Parser<TToken, IEnumerable<TTree>> Many1<TTree>(Parser<TToken, TTree> parser)
        {
            return Count(1, null, parser);
        }

        public static Parser<TToken, IEnumerable<TTree>> Count<TTree>(int count, Parser<TToken, TTree> parser)
        {
            return Count(count, count, parser);
        }

        public static Parser<TToken, IEnumerable<TTree>> Count<TTree>(int min, int? max, Parser<TToken, TTree> parser)
        {
            min = Math.Max(0, min);
            max = max ?? int.MaxValue;

            if (min > max)
                return Fail<IEnumerable<TTree>>();

            return consList =>
            {
                List<TTree> trees = new List<TTree>();
                Result<TToken, TTree> result;
                int count = 0;

                if (max > 0)
                    do
                    {
                        result = parser(consList);

                        if (result != null)
                        {
                            trees.Add(result.Tree);
                            consList = result.Rest;
                            count++;
                        }
                    }
                    while (result != null && count < max);

                if (count >= min)
                    return new Result<TToken, IEnumerable<TTree>>(trees, consList);
                else
                    return null;
            };
        }

        public static Parser<TToken, IEnumerable<TTree>> Sequence<TTree>(IEnumerable<Parser<TToken, TTree>> parsers)
        {
            return consList =>
            {
                List<TTree> trees = new List<TTree>();
                Result<TToken, TTree> result;

                foreach (var parser in parsers)
                {
                    result = parser(consList);

                    if (result == null)
                        return null;

                    trees.Add(result.Tree);
                    consList = result.Rest;
                }

                return new Result<TToken, IEnumerable<TTree>>(trees, consList);
            };
        }

        public static Parser<TToken, TTree> PrefixedBy<TPrefix, TTree>(Parser<TToken, TPrefix> prefix,
                                                                       Parser<TToken, TTree> parser)
        {
            return from p in prefix
                   from x in parser
                   select x;
        }

        public static Parser<TToken, TTree> Between<TOpen, TClose, TTree>(Parser<TToken, TOpen> open,
                                                                          Parser<TToken, TClose> close,
                                                                          Parser<TToken, TTree> parser)
        {
            return from o in open
                   from x in parser
                   from c in close
                   select x;
        }

        public static Parser<TToken, IEnumerable<TTree>> SepBy<TTree, TSep>(Parser<TToken, TTree> parser,
                                                                            Parser<TToken, TSep> sep)
        {
            return SepBy(0, parser, sep);
        }

        public static Parser<TToken, IEnumerable<TTree>> SepBy1<TTree, TSep>(Parser<TToken, TTree> parser,
                                                                             Parser<TToken, TSep> sep)
        {
            return SepBy(1, parser, sep);
        }

        public static Parser<TToken, IEnumerable<TTree>> SepBy<TTree, TSep>(int minItemCount,
                                                                            Parser<TToken, TTree> parser,
                                                                            Parser<TToken, TSep> sep)
        {
            if (minItemCount <= 0)
                return Choice(SepBy(1, parser, sep),
                              Succeed(Enumerable.Empty<TTree>()));
            else
                return from first in parser
                       from rest in
                           Count(minItemCount - 1,
                                 null,
                                 from s in sep
                                 from v in parser
                                 select v)
                       select new[] { first }.Concat(rest);
        }

        public static Parser<TToken, UnitType> NotFollowedBy<TTree>(Parser<TToken, TTree> parser)
        {
            return consList => parser(consList) != null ? null : new Result<TToken, UnitType>(UnitType.Unit, consList);
        }

        public static Parser<TToken, UnitType> Eof
        {
            get { return NotFollowedBy(Token); }
        }

        public static Parser<TToken, TTree> Lazy<TTree>(Func<Parser<TToken, TTree>> thunk)
        {
            return consList => thunk()(consList);
        }

        public static IEnumerable<T> LazySeq<T>(params Func<T>[] thunks)
        {
            return thunks.Select(t => t());
        }
    }
}
