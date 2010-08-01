using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.ConsLists;
using ParserCombinators.Util;

namespace ParserCombinators
{
    public class Parsers<TToken>
    {
        public static Parser<TToken, TToken> Token
        {
            get
            {
                return consList =>
                    !consList.IsEmpty ? ResultSet<TToken, TToken>.SingleResult(consList.Head, consList.Tail) :
                                        ResultSet<TToken, TToken>.Empty();
            }
        }

        public static Parser<TToken, TValue> Succeed<TValue>(TValue value)
        {
            return consList => ResultSet<TToken, TValue>.SingleResult(value, consList);
        }

        public static Parser<TToken, TValue> Fail<TValue>()
        {
            return consList => ResultSet<TToken, TValue>.Empty();
        }

        public static Parser<TToken, TValue> Choice<TValue>(params Parser<TToken, TValue>[] choices)
        {
            return consList =>
                new ResultSet<TToken, TValue>(
                    choices.SelectMany(parser => parser(consList)));
        }

        public static Parser<TToken, TValue> Option<TValue>(TValue defaultValue, Parser<TToken, TValue> parser)
        {
            return Choice(parser, Succeed(defaultValue));
        }

        public static Parser<TToken, TValue?> OptionNullable<TValue>(Parser<TToken, TValue> parser)
            where TValue : struct
        {
            return Option(null,
                          from x in parser select (TValue?)x);
        }

        public static Parser<TToken, IEnumerable<TValue>> Many<TValue>(Parser<TToken, TValue> parser)
        {
            return Count(0, null, parser);
        }

        public static Parser<TToken, IEnumerable<TValue>> Many1<TValue>(Parser<TToken, TValue> parser)
        {
            return Count(1, null, parser);
        }

        public static Parser<TToken, IEnumerable<TValue>> Count<TValue>(int count, Parser<TToken, TValue> parser)
        {
            return Count(count, count, parser);
        }

        public static Parser<TToken, IEnumerable<TValue>> Count<TValue>(int min, int? max, Parser<TToken, TValue> parser)
        {
            //min = Math.Max(0, min);
            //max = max ?? int.MaxValue;

            //if (min > max)
            //    return Fail<IEnumerable<TValue>>();

            //return consList =>
            //{
            //    List<TValue> values = new List<TValue>();
            //    Result<TToken, TValue> result;
            //    int count = 0;

            //    if (max > 0)
            //        do
            //        {
            //            result = parser(consList);

            //            if (result != null)
            //            {
            //                values.Add(result.Value);
            //                consList = result.Rest;
            //                count++;
            //            }
            //        }
            //        while (result != null && count < max);

            //    if (count >= min)
            //        return new Result<TToken, IEnumerable<TValue>>(values, consList);
            //    else
            //        return null;
            //};


            min = Math.Max(0, min);
            max = max ?? int.MaxValue;

            if (min > max)
                return Fail<IEnumerable<TValue>>();


            //if (max == 0)
            //    return Succeed(Enumerable.Empty<TValue>());
            //else
            //    return from v in parser
            //           from vs in Count(min - 1, max - 1, parser)
            //           select new[] { v }.Concat(vs);


            return consList =>
            {
                List<TValue> values = new List<TValue>();
                Result<TToken, TValue> result;
                int count = 0;

                if (max > 0)
                    do
                    {
                        result = parser(consList).FirstOrDefault();

                        if (result != null)
                        {
                            values.Add(result.Value);
                            consList = result.Rest;
                            count++;
                        }
                    }
                    while (result != null && count < max);

                if (count >= min)
                    return ResultSet<TToken, IEnumerable<TValue>>.SingleResult(values, consList);
                else
                    return ResultSet<TToken, IEnumerable<TValue>>.Empty();
            };
        }

        public static Parser<TToken, IEnumerable<TValue>> Sequence<TValue>(IEnumerable<Parser<TToken, TValue>> parsers)
        {
            //return consList =>
            //{
                //List<TValue> values = new List<TValue>();
                //Result<TToken, TValue> result;

                //foreach (var parser in parsers)
                //{
                //    result = parser(consList);

                //    if (result == null)
                //        return null;

                //    values.Add(result.Value);
                //    consList = result.Rest;
                //}

                //return new Result<TToken, IEnumerable<TValue>>(values, consList);



                //List<TValue> values = new List<TValue>();
                //Result<TToken, TValue> result;

                //foreach (var parser in parsers)
                //{
                //    result = parser(consList).FirstOrDefault();

                //    if (result == null)
                //        return ResultSet<TToken, IEnumerable<TValue>>.Empty();

                //    values.Add(result.Value);
                //    consList = result.Rest;
                //}

                //return ResultSet<TToken, IEnumerable<TValue>>.SingleResult(values, consList);



                //Console.WriteLine(r.Rest.AsEnumerable().Select(e => e.ToString()).ToArray().JoinStrings(", "));

                //var stepResultSet = ResultSet<TToken, IEnumerable<TValue>>.SingleResult(new List<TValue>(), consList);

                //foreach (var parser in parsers)
                //{
                //    stepResultSet = new ResultSet<TToken, IEnumerable<TValue>>(
                //        stepResultSet.SelectMany(result =>
                //            parser(result.Rest).Select(r =>
                //            {
                //                List<TValue> newList = result.Value.ToList();
                //                newList.Add(r.Value);
                //                return new Result<TToken, IEnumerable<TValue>>(newList, r.Rest);
                //            })));

                //    if (!stepResultSet.Any())
                //        return ResultSet<TToken, IEnumerable<TValue>>.Empty();
                //}

                //return stepResultSet;
            //};

            if (parsers.Any())
                return from v in parsers.First()
                       from vs in Sequence(parsers.Skip(1))
                       select new[] { v }.Concat(vs);
            else
                return Succeed(Enumerable.Empty<TValue>());
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

        public static Parser<TToken, IEnumerable<TValue>> SepBy<TValue, TSep>(Parser<TToken, TValue> parser,
                                                                              Parser<TToken, TSep> sep)
        {
            return SepBy(0, parser, sep);
        }

        public static Parser<TToken, IEnumerable<TValue>> SepBy1<TValue, TSep>(Parser<TToken, TValue> parser,
                                                                               Parser<TToken, TSep> sep)
        {
            return SepBy(1, parser, sep);
        }

        public static Parser<TToken, IEnumerable<TValue>> SepBy<TValue, TSep>(int minItemCount,
                                                                              Parser<TToken, TValue> parser,
                                                                              Parser<TToken, TSep> sep)
        {
            if (minItemCount <= 0)
                return Choice(SepBy(1, parser, sep),
                              Succeed(Enumerable.Empty<TValue>()));
            else
                return from first in parser
                       from rest in
                           Count(minItemCount - 1,
                                 from s in sep
                                 from v in parser
                                 select v)
                       select new[] { first }.Concat(rest);
        }

        public static Parser<TToken, UnitType> NotFollowedBy<TValue>(Parser<TToken, TValue> parser)
        {
            return consList =>
                parser(consList).Any() ? ResultSet<TToken, UnitType>.Empty() :
                                         ResultSet<TToken, UnitType>.SingleResult(UnitType.Unit, consList);
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
