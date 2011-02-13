using System;
using System.Collections.Generic;
using System.Linq;

namespace RegexParser.Util
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<R> Zip<A, B, R>(this IEnumerable<A> a, IEnumerable<B> b,
                                                  Func<A, B, R> func)
        {
            IEnumerator<A> enumA = a.GetEnumerator();
            IEnumerator<B> enumB = b.GetEnumerator();

            while (enumA.MoveNext() && enumB.MoveNext())
                yield return func(enumA.Current, enumB.Current);
        }

        public static IEnumerable<R> Zip3<A, B, C, R>(this IEnumerable<A> a, IEnumerable<B> b, IEnumerable<C> c,
                                                      Func<A, B, C, R> func)
        {
            return Zip(Zip<A, B, Func<C, R>>(a, b, (aa, bb) => cc => func(aa, bb, cc)),
                       c,
                       (f, ccc) => f(ccc));
        }
    }
}
