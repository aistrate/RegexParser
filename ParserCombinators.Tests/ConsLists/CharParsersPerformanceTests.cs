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
    public static class CharParsersPerformanceTests
    {
        public static void AnyCharTest()
        {
            charParserTest(new CharParsers().AnyChar,
                           200, 1000000);
            
            // 12.80 sec. (maxItemCount = 1,000,000)
            // 26.09 sec. (maxItemCount = 2,000,000)
            // for traversal, time grows linearly with 'maxItemCount';
        }

        public static void OneOfTest()
        {
            charParserTest(new CharParsers().OneOf("0123456789"),
                           200, 1000000);

            // 61.08 sec. (maxItemCount = 1,000,000)
        }

        public static void ManyCharsTest()
        {
            charParserTest(new CharParsers().Many1(new CharParsers().AnyChar),
                           200, 1000000);

            // 17.97 sec. (maxItemCount = 1,000,000)
            // 37.55 sec. (maxItemCount = 2,000,000)
        }

        private static void charParserTest<TValue>(Parser<char, TValue> parser, int times, int maxItemCount)
        {
            string str = new string(EnumerablePerformanceTests.GetDigitChars(maxItemCount).ToArray());
            IConsList<char> consList = new ArrayConsList<char>(str);


            DateTime start = DateTime.Now;

            IConsList<char> rest = null;
            for (int i = 0; i < times; i++)
            {
                Result<char, TValue> result = parser(consList);

                while (result != null)
                {
                    rest = result.Rest;
                    result = parser(result.Rest);
                }
            }
            
            if (rest != null && !rest.IsEmpty)
                Console.WriteLine("The remaining list is NOT empty.\n");

            TimeSpan time = DateTime.Now - start;
            TimeSpan timePerCycle = new TimeSpan(0, 0, 0, 0, (int)(time.TotalMilliseconds / times));

            Console.WriteLine(time);
            Console.WriteLine();
            Console.WriteLine("{0}  per cycle", timePerCycle);
            Console.WriteLine();
        }
    }
}
