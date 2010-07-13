using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.Tests.ConsLists
{
    /// <summary>
    /// Does NOT contain unit tests, but performance-comparison tests to be run from a console.
    /// </summary>
    public static class EnumerablePerformanceTests
    {
        public static void TestEnumerableChars()
        {
            const int times = 200;
            const int max = 1000000;

            Console.WriteLine("Initialization:");
            DateTime start = DateTime.Now;

            IEnumerable<char> charEnum = GetDigitChars(max);
            char[] charArr = GetDigitChars(max).ToArray();
            List<char> charList = GetDigitChars(max).ToArray().ToList();
            string charString = new string(GetDigitChars(max).ToArray());

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
            // 0.011 sec.

            Console.WriteLine("char[]:");
            testEnumerable(charArr, times);
            // 4.08 sec. / 2.75 sec. (diff = 1.33 sec.)

            Console.WriteLine("string:");
            testEnumerable(charString, times);
            // 4.14 sec. / 2.91 sec. (diff = 1.23 sec.)

            Console.WriteLine("List<char>:");
            testEnumerable(charList, times);
            // 4.53 sec. / 3.45 sec. (diff = 1.08 sec.)

            Console.WriteLine("IEnumerable<char>:");
            testEnumerable(charEnum, times);
            // 4.90 sec. / 3.58 sec. (diff = 1.32 sec.)
            // 18.45 sec. / 16.34 sec.
        }

        public static void TestEnumerableInts()
        {
            const int times = 200;
            const int max = 1000000;

            IEnumerable<int> naturalNumbers;

            naturalNumbers = getNaturalNumbers(max);
            Console.WriteLine("IEnumerable<int>:");
            testEnumerable(naturalNumbers, times);
            // 3.83 sec. / 2.61 sec. (diff = 1.22 sec.)

            naturalNumbers = getNaturalNumbers(max).ToList();
            Console.WriteLine("List<int>:");
            testEnumerable(naturalNumbers, times);
            // 4.02 sec. / 2.95 sec. (diff = 1.07 sec.)

            naturalNumbers = getNaturalNumbers(max).ToArray();
            Console.WriteLine("int[]:");
            testEnumerable(naturalNumbers, times);
            // 4.09 sec. / 3.11 sec. (diff = 0.98 sec.)
        }

        public static void TestEnumerableCharsWithSkip()
        {
            const int times = 1;
            const int max = 700;

            char[] charArr = GetDigitChars(max).ToArray();
            string charString = new string(GetDigitChars(max).ToArray());

            Console.WriteLine("char[]:");
            testEnumerableWithSkip(charArr, times);
            // 5.4 sec.

            Console.WriteLine("string:");
            testEnumerableWithSkip(charString, times);
            // 5.38 sec.
        }

        public static void TestEnumerableIntsWithSkip()
        {
            const int times = 1;
            const int max = 700;

            IEnumerable<int> naturalNumbers;

            naturalNumbers = getNaturalNumbers(max).ToArray();
            Console.WriteLine("int[]:");
            testEnumerableWithSkip(naturalNumbers, times);
            // 5.36 sec.

            naturalNumbers = getNaturalNumbers(max);
            Console.WriteLine("IEnumerable<int>:");
            testEnumerableWithSkip(naturalNumbers, times);
            // 5.38 sec.
        }

        public static void TestEnumerableCharsWithSubstring()
        {
            const int times = 1;
            const int max = 50000;

            string str = new string(GetDigitChars(max).ToArray());


            Console.WriteLine("substring:");
            DateTime start = DateTime.Now;

            for (int i = 0; i < times; i++)
            {
                string newStr = str;

                while (newStr.Length > 0)
                {
                    char x = newStr[0];
                    newStr = newStr.Substring(1);
                }
            }

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
            //  2.35 sec  (max =  50000)
            // 13.39 sec. (max = 100000)
            // 58    sec. (max = 200000)
            // time grows by the square of 'max'
        }

        public static void TestEnumerableCharsWithCharBuffer()
        {
            const int times = 200;
            const int max = 1000000;

            string str = new string(GetDigitChars(max).ToArray());

            Console.WriteLine("Init:");
            DateTime start = DateTime.Now;

            CharBuffer charBuffer = new CharBuffer(str);

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();

            
            Console.WriteLine("CharBuffer:");
            start = DateTime.Now;

            for (int i = 0; i < times; i++)
            {
                CharBuffer newCharBuffer = charBuffer;

                while (!newCharBuffer.IsAtEnd)
                {
                    char x = newCharBuffer.Current;
                    newCharBuffer = newCharBuffer.MoveNext();
                }
            }

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
            // 0    +  3.13 sec. (max =   1,000,000)
            // 0.02 + 31.63 sec. (max =  10,000,000)
            // 0.22 + ?          (max = 100,000,000)
            // time grows liniarly with 'max'
        }

        public static void TestEnumerableCharsWithLinkedList()
        {
            const int times = 200;
            const int max = 1000000;

            string str = new string(GetDigitChars(max).ToArray());
            
            Console.WriteLine("Init:");
            DateTime start = DateTime.Now;
            
            LinkedList<char> linkedList = new LinkedList<char>(str);
            
            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();


            Console.WriteLine("LinkedList:");
            start = DateTime.Now;

            for (int i = 0; i < times; i++)
            {
                LinkedListNode<char> node = linkedList.First;

                while (node != null)
                {
                    char x = node.Value;
                    node = node.Next;
                }
            }

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
            //  0.25 +  1.34 sec. (max =  1,000,000)
            //  5.00 + 13.39 sec. (max = 10,000,000)
            // 10.45 + 26.77 sec. (max = 20,000,000)
            // for traversal, time grows liniarly with 'max'
        }

        private static void testEnumerable<T>(IEnumerable<T> items, int times)
        {
            DateTime start = DateTime.Now;

            for (int i = 0; i < times; i++)
                foreach (T e in items)
                {
                    T x = e;
                }

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
        }

        private static void testEnumerableWithSkip<T>(IEnumerable<T> items, int times)
        {
            DateTime start = DateTime.Now;

            for (int i = 0; i < times; i++)
            {
                IEnumerable<T> coll = items;

                while (!coll.FirstOrDefault().Equals(default(T)))
                {
                    T x = coll.First();
                    coll = coll.Skip(1);
                }
            }

            Console.WriteLine(DateTime.Now - start);
            Console.WriteLine();
        }

        private static IEnumerable<int> getNaturalNumbers(int max)
        {
            for (int n = 1; n <= max; n++)
                yield return n;
        }

        public static IEnumerable<char> GetDigitChars(int max)
        {
            IEnumerable<char> digitChars = Enumerable.Repeat("1234567890", (max / 10) + 1)
                                                     .SelectMany(s => s.ToCharArray())
                                                     .Take(max);

            foreach (char d in digitChars)
                yield return d;
        }
    }
}
