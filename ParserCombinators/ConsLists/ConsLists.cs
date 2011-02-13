using System.Collections.Generic;

namespace ParserCombinators.ConsLists
{
    /// <summary>
    /// Extension methods for ConsLists.
    /// </summary>
    public static class ConsLists
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

        public static SimpleConsList<T> Cons<T>(this T value, SimpleConsList<T> list)
        {
            return new SimpleConsList<T>(value, list);
        }
    }
}
