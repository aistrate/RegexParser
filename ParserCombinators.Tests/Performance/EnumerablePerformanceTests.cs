using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.Tests.Performance
{
    /// <summary>
    /// Does NOT contain unit tests, but performance-comparison tests to be run from a console.
    /// </summary>
    public static class EnumerablePerformanceTests
    {
        public static void TestContainsCharBySize()
        {
            compareContainsChar(100, 1000000);

            compareContainsChar(100000, 1000);

            compareContainsChar(1000000, 100);

            compareContainsChar(2000000, 50);

            compareContainsChar(2500000, 40);

            compareContainsChar(3333333, 30);

            compareContainsChar(4000000, 25);

            compareContainsChar(5000000, 20);

            compareContainsChar(10000000, 10);

            compareContainsChar(20000000, 5);

            //String     with Contains (       100 times, size =  1,000,000):    2.344 sec.
            //String     with IndexOf  (       100 times, size =  1,000,000):    0.156 sec.
            //Char Array with Contains (       100 times, size =  1,000,000):    0.219 sec.
            //Char Array with IndexOf  (       100 times, size =  1,000,000):    0.234 sec.

            //String     with Contains (   100,000 times, size =      1,000):    2.391 sec.
            //String     with IndexOf  (   100,000 times, size =      1,000):    0.141 sec.
            //Char Array with Contains (   100,000 times, size =      1,000):    0.250 sec.
            //Char Array with IndexOf  (   100,000 times, size =      1,000):    0.219 sec.

            //String     with Contains ( 1,000,000 times, size =        100):    2.438 sec.
            //String     with IndexOf  ( 1,000,000 times, size =        100):    0.156 sec.
            //Char Array with Contains ( 1,000,000 times, size =        100):    0.766 sec.
            //Char Array with IndexOf  ( 1,000,000 times, size =        100):    0.266 sec.

            //String     with Contains ( 2,000,000 times, size =         50):    2.563 sec.
            //String     with IndexOf  ( 2,000,000 times, size =         50):    0.203 sec.
            //Char Array with Contains ( 2,000,000 times, size =         50):    1.328 sec.
            //Char Array with IndexOf  ( 2,000,000 times, size =         50):    0.297 sec.

            //String     with Contains ( 2,500,000 times, size =         40):    2.641 sec.
            //String     with IndexOf  ( 2,500,000 times, size =         40):    0.203 sec.
            //Char Array with Contains ( 2,500,000 times, size =         40):    1.609 sec.
            //Char Array with IndexOf  ( 2,500,000 times, size =         40):    0.328 sec.

            //String     with Contains ( 3,333,333 times, size =         30):    2.766 sec.
            //String     with IndexOf  ( 3,333,333 times, size =         30):    0.234 sec.
            //Char Array with Contains ( 3,333,333 times, size =         30):    2.094 sec.
            //Char Array with IndexOf  ( 3,333,333 times, size =         30):    0.375 sec.

            //String     with Contains ( 4,000,000 times, size =         25):    2.813 sec.
            //String     with IndexOf  ( 4,000,000 times, size =         25):    0.266 sec.
            //Char Array with Contains ( 4,000,000 times, size =         25):    2.453 sec.
            //Char Array with IndexOf  ( 4,000,000 times, size =         25):    0.422 sec.

            //String     with Contains ( 5,000,000 times, size =         20):    2.953 sec.
            //String     with IndexOf  ( 5,000,000 times, size =         20):    0.297 sec.
            //Char Array with Contains ( 5,000,000 times, size =         20):    3.000 sec.
            //Char Array with IndexOf  ( 5,000,000 times, size =         20):    0.469 sec.

            //String     with Contains (10,000,000 times, size =         10):    3.641 sec.
            //String     with IndexOf  (10,000,000 times, size =         10):    0.453 sec.
            //Char Array with Contains (10,000,000 times, size =         10):    5.813 sec.
            //Char Array with IndexOf  (10,000,000 times, size =         10):    0.719 sec.

            //String     with Contains (20,000,000 times, size =          5):    4.969 sec.
            //String     with IndexOf  (20,000,000 times, size =          5):    0.563 sec.
            //Char Array with Contains (20,000,000 times, size =          5):   11.266 sec.
            //Char Array with IndexOf  (20,000,000 times, size =          5):    1.063 sec.
        }

        private static void compareContainsChar(int times, int max)
        {
            char c = 'x';
            IEnumerable<char> charEnum = RepeatDigitChars(max);

            char[] charArr = charEnum.ToArray();
            string charString = new string(charArr);

            testContainsChar(() => charString.Contains(c), times, max, "String    ", "Contains");
            testContainsChar(() => charString.IndexOf(c) >= 0, times, max, "String    ", "IndexOf ");

            testContainsChar(() => charArr.Contains(c), times, max, "Char Array", "Contains");
            testContainsChar(() => Array.IndexOf(charArr, c) >= 0, times, max, "Char Array", "IndexOf ");

            Console.WriteLine();
        }

        private static void testContainsChar(Func<bool> contains, int times, int max, string seqName, string funcName)
        {
            DateTime start = DateTime.Now;

            for (int i = 0; i < times; i++)
                contains();

            TimeSpan time = DateTime.Now - start;

            Console.WriteLine("{0} with {1} ({2,10:#,##0} times, size = {3,10:#,##0}): {4,8:#0.000} sec.",
                              seqName, funcName, times, max, time.TotalMilliseconds / 1000);
        }

        public static void TestEnumerableChars()
        {
            const int times = 200;
            const int max = 1000000;

            Console.WriteLine("Initialization:");
            DateTime start = DateTime.Now;

            IEnumerable<char> charEnum = RepeatDigitChars(max);
            char[] charArr = RepeatDigitChars(max).ToArray();
            List<char> charList = RepeatDigitChars(max).ToArray().ToList();
            string charString = new string(RepeatDigitChars(max).ToArray());

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

            char[] charArr = RepeatDigitChars(max).ToArray();
            string charString = new string(RepeatDigitChars(max).ToArray());

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

            string str = new string(RepeatDigitChars(max).ToArray());


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
            //  2.35 sec. (max =  50000)
            // 13.39 sec. (max = 100000)
            // 58    sec. (max = 200000)
            // time grows by the square of 'max'
        }

        public static void TestEnumerableCharsWithCharBuffer()
        {
            const int times = 200;
            const int max = 1000000;

            string str = new string(RepeatDigitChars(max).ToArray());

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
            // time grows linearly with 'max'
        }

        public static void TestEnumerableCharsWithLinkedList()
        {
            const int times = 200;
            const int max = 1000000;

            string str = new string(RepeatDigitChars(max).ToArray());
            
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
            // for traversal, time grows linearly with 'max'
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

        public static IEnumerable<char> RepeatDigitChars(int finalLength)
        {
            return RepeatChars("1234567890", finalLength);
        }

        public static IEnumerable<char> RepeatChars(string repeatChars, int finalLength)
        {
            IEnumerable<char> digitChars = Enumerable.Repeat(repeatChars, (finalLength / repeatChars.Length) + 1)
                                                     .SelectMany(s => s.ToCharArray())
                                                     .Take(finalLength);

            foreach (char d in digitChars)
                yield return d;
        }
    }
}
