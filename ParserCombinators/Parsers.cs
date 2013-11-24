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

        // Try to apply the parsers in the 'choices' list in order, until one succeeds.
        // Return the tree returned by the succeeding parser.
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

        // Try to apply 'parser'. If 'parser' succeeds, return the tree returned by it,
        // otherwise return 'defaultTree' (without consuming input).
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

        // Apply 'parser' zero or more times. Return a list of the trees returned by 'parser'.
        public static Parser<TToken, IEnumerable<TTree>> Many<TTree>(Parser<TToken, TTree> parser)
        {
            return Count(0, null, parser);
        }

        // Apply 'parser' one or more times. Return a list of the trees returned by 'parser'.
        public static Parser<TToken, IEnumerable<TTree>> Many1<TTree>(Parser<TToken, TTree> parser)
        {
            return Count(1, null, parser);
        }

        // Apply 'parser' an exact number of times ('count'). Return a list of the trees returned by 'parser'.
        public static Parser<TToken, IEnumerable<TTree>> Count<TTree>(int count, Parser<TToken, TTree> parser)
        {
            return Count(count, count, parser);
        }

        // Apply 'parser' between 'min' and 'max' times (where max==null means infinity).
        // Return a list of the trees returned by 'parser'.
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

        // Apply the parsers in the 'parsers' list, each consuming more input. Return a list of the trees returned by the parsers.
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

        // Apply 'prefix', followed by 'parser'. Return the tree returned by 'parser'.
        public static Parser<TToken, TTree> PrefixedBy<TPrefix, TTree>(Parser<TToken, TPrefix> prefix,
                                                                       Parser<TToken, TTree> parser)
        {
            return from p in prefix
                   from x in parser
                   select x;
        }

        // Apply 'open', followed by 'parser' and 'close'. Return the tree returned by 'parser'.
        public static Parser<TToken, TTree> Between<TOpen, TClose, TTree>(Parser<TToken, TOpen> open,
                                                                          Parser<TToken, TClose> close,
                                                                          Parser<TToken, TTree> parser)
        {
            return from o in open
                   from x in parser
                   from c in close
                   select x;
        }

        // Parse zero or more occurrences of 'parser', separated by 'sep'. Return a list of the trees returned by 'parser'.
        public static Parser<TToken, IEnumerable<TTree>> SepBy<TTree, TSep>(Parser<TToken, TTree> parser,
                                                                            Parser<TToken, TSep> sep)
        {
            return SepBy(0, parser, sep);
        }

        // Parse one or more occurrences of 'parser', separated by 'sep'. Return a list of the trees returned by 'parser'.
        public static Parser<TToken, IEnumerable<TTree>> SepBy1<TTree, TSep>(Parser<TToken, TTree> parser,
                                                                             Parser<TToken, TSep> sep)
        {
            return SepBy(1, parser, sep);
        }

        // Parse a minimum of 'minItemCount' occurrences of 'parser', separated by 'sep'.
        // Return a list of the trees returned by 'parser'.
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

        // Succeed only when 'parser' fails. Do not consume any input.
        public static Parser<TToken, UnitType> NotFollowedBy<TTree>(Parser<TToken, TTree> parser)
        {
            return consList => parser(consList) != null ? null : new Result<TToken, UnitType>(UnitType.Unit, consList);
        }

        // Only succeed at the end of the input.
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
