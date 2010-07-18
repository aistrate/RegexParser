using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.ConsLists
{
    public static class ConsList
    {
        public static IEnumerable<T> AsEnumerable<T>(this IConsList<T> source)
        {
            while (!source.IsEmpty)
            {
                yield return source.Head;
                source = source.Tail;
            }
        }

        public static IConsList<T> Drop<T>(this IConsList<T> source, int count)
        {
            IConsList<T> result = source;
            
            for (int i = 0; i < count && !result.IsEmpty; i++)
                result = result.Tail;

            return result;
        }
    }
}
