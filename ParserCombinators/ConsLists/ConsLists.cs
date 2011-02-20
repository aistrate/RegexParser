using System;
using System.Collections.Generic;
using System.Linq;
using ParserCombinators.Util;

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

        private const bool isAssertEnabled = true;

        internal static void AssertNotEmpty<T>(this IConsList<T> consList, ConsOp consOp)
        {
            if (isAssertEnabled && consList.IsEmpty)
                throw new ApplicationException(
                    string.Format("{0}: could not perform {1} operation because Cons List is empty.",
                                  getGenericTypeName(consList.GetType()),
                                  consOp));
        }

        private static string getGenericTypeName(Type type)
        {
            string name = type.Name;

            if (name.IndexOf('`') >= 0)
                name = string.Format("{0}<{1}>",
                                     name.Split('`')[0],
                                     type.GetGenericArguments()
                                         .Select(t => getGenericTypeName(t))
                                         .JoinStrings(", "));

            return name;
        }
    }

    internal enum ConsOp
    {
        Head,
        Tail,
    }
}
