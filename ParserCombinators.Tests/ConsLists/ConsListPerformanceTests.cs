using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.ConsLists;

namespace ParserCombinators.Tests.ConsLists
{
    /// <summary>
    /// Does NOT contain unit tests, but performance-comparison tests to be run from a console.
    /// </summary>
    public static class ConsListPerformanceTests
    {
        public static void StringConsList()
        {
            consListTest(s => new StringConsList(s), 1, 200000);
            
            // 0 +  2.48 sec  (max =  50000)
            // 0 + 13.20 sec. (max = 100000)
            // 0 + 57.44 sec. (max = 200000)
            // time grows by the square of 'max'
        }

        public static void LinkedConsList()
        {
            consListTest(s => new LinkedConsList<char>(s), 200, 1000000);
            
            // 0.27 +  7.83 sec. (maxItemCount =  1,000,000)
            // 0.70 + 15.63 sec. (maxItemCount =  2,000,000)
            // 5.06 + 81.31 sec. (maxItemCount = 10,000,000)
            // for traversal, time grows liniarly with 'maxItemCount';
            // LinkedConsList<T> is about 5.8 times slower than a simple LinkedList<T> traversal
            // (EnumerablePerformanceTests.TestEnumerableCharsWithLinkedList)
        }

        public static void ArrayConsList()
        {
            consListTest(s => new ArrayConsList<char>(s), 200, 1000000);

            // 0.03 +  6.03 sec. (maxItemCount =  1,000,000)
            // 0.08 + 12.56 sec. (maxItemCount =  2,000,000)
            // 0.55 + 62.27 sec. (maxItemCount = 10,000,000)
            // for traversal, time grows liniarly with 'maxItemCount';
            // ArrayConsList<T> is about 2 times slower than using CharBuffer
            // (EnumerablePerformanceTests.TestEnumerableCharsWithCharBuffer)
        }

        private static void consListTest(Func<string, IConsList<char>> createConsList,
                                         int times, int maxItemCount)
        {
            string str = new string(EnumerablePerformanceTests.GetDigitChars(maxItemCount).ToArray());

            Console.WriteLine("Init:");
            DateTime start = DateTime.Now;

            IConsList<char> consList = createConsList(str);

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();


            Console.WriteLine("ConsList:");
            start = DateTime.Now;

            for (int i = 0; i < times; i++)
            {
                IConsList<char> newConsList = consList;

                while (!newConsList.IsEmpty)
                {
                    char x = newConsList.Head;
                    newConsList = newConsList.Tail;
                }
            }

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
        }
    }
}
