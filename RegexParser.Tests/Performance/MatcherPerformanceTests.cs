using System;
using System.Diagnostics;
using System.Linq;
using ParserCombinators.Tests.Performance;
using ParserCombinators.Util;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Performance
{
    public static class MatcherPerformanceTests
    {
        public static void VeryLongMatches()
        {
            const int times = 1, inputLength = 1000000;

            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string lowercaseChars = EnumerablePerformanceTests.RepeatChars(alphabet, inputLength)
                                                              .AsString();

            //Console.WriteLine("(Short input)");
            //testRegexMatches("x", @".", times);
            //// 0.171 sec.

            testRegexMatches(lowercaseChars, @"", times);
            //  2.138 sec. (inputLength =  1,000,000)
            // 37.968 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"k", times);
            //  2.907 sec. (inputLength =  1,000,000)
            // 30.292 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"\w", times);
            //  8.315 sec. (inputLength =  1,000,000)   (!?)
            // 16.459 sec. (inputLength =  2,000,000)
            // 25.295 sec. (inputLength =  3,000,000)
            // 41.248 sec. (inputLength =  5,000,000)
            // 83.437 sec. (inputLength = 10,000,000)   (64 bytes/match)

            testRegexMatches(lowercaseChars, @"\w{1000}", times);
            //  0.669 sec. (inputLength =  1,000,000)
            //  6.272 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"(\w{1000})+", times);
            //  0.857 sec. (inputLength =  1,000,000)
            //  9.044 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"(\w{10000})+", times);
            //  0.898 sec. (inputLength =  1,000,000)
            //  9.242 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"(\w{20000})+", times);
            //  1.173 sec. (inputLength =  1,000,000)
            // 12.828 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"(\w{100000})+", times);
            //  1.288 sec. (inputLength =  1,000,000)
            // 13.453 sec. (inputLength = 10,000,000)

            testRegexMatches(lowercaseChars, @"(\w{10001})+", times);
            // 12.556 sec. (inputLength =  1,000,000)   (!?)
            // 18.828 sec. (inputLength = 10,000,000)


            testRegexMatches(lowercaseChars, alphabet, times);

            testRegexMatches(lowercaseChars, alphabet.Substring(0, alphabet.Length / 2), times);

            testRegexMatches(lowercaseChars, string.Format("({0})+", alphabet), times);

            testRegexMatches(lowercaseChars, @"\w{1000000}", times);

            testRegexMatches(lowercaseChars, @"\w+", times);

            testRegexMatches(lowercaseChars, @"\w+n", times);

            testRegexMatches(lowercaseChars, @"\w+e", times);

            testRegexMatches(lowercaseChars + "7", @"\w+7", times);

            testRegexMatches(lowercaseChars + "7", @"\w+?7", times);
        }

        private const bool useMemoryProfiler = false;

        private static void testRegexMatches(string input, string pattern, int times)
        {
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            MemoryProfiler memoryProfiler;

            if (useMemoryProfiler)
                memoryProfiler = MemoryProfiler.StartNew();

            Stopwatch stopwatch = Stopwatch.StartNew();

            MatchCollection2 matches = null;
            //Msoft.MatchCollection matches = null;

            for (int i = 0; i < times; i++)
                matches = new Regex2(pattern, AlgorithmType.Backtracking).Matches(input);
                //matches = new Msoft.Regex(pattern).Matches(input);

            if (useMemoryProfiler)
                memoryProfiler.Reset();

            Console.WriteLine("Matches: {0:#,##0}", matches.Count);

            //string v;
            //foreach (var m in matches.Cast<Match2>())
            ////foreach (var m in matches.Cast<Msoft.Match>())
            //    v = m.Value;

            decimal elapsed = ((decimal)stopwatch.ElapsedMilliseconds) / 1000;

            if (useMemoryProfiler)
            {
                long deltaBefore = memoryProfiler.DeltaValue;
                memoryProfiler.CollectGC();
                long deltaAfter = memoryProfiler.DeltaValue;

                if (matches.Count > 0)
                    Console.WriteLine("Last:    {0:#,##0} chars", matches[matches.Count - 1].Value.Length);

                Console.WriteLine("Memory:  {0,10:#,##0} bytes", deltaBefore);
                Console.WriteLine("AfterGC: {0,10:#,##0} bytes", deltaAfter);
            }

            Console.WriteLine("Time:    {0:#0.000} sec.\n", elapsed);
        }
    }
}
