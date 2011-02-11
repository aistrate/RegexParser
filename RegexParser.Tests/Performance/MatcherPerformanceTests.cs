using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using ParserCombinators.Tests.Performance;
using ParserCombinators.Util;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Performance
{
    public static class MatcherPerformanceTests
    {
        public static void VeryLongMatches()
        {
            const int times = 1, inputLength = 1000000;

            string lowercaseChars = EnumerablePerformanceTests.RepeatChars("abcdefghijklmnopqrstuvwxyz", inputLength)
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
        }

        private static MatchCollection2 testRegexMatches(string input, string pattern, int times)
        {
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            MemoryProfiler memoryProfiler = MemoryProfiler.StartNew();
            Stopwatch stopwatch = Stopwatch.StartNew();

            MatchCollection2 matches = null;
            
            for (int i = 0; i < times; i++)
                matches = new Regex2(pattern, AlgorithmType.Backtracking).Matches(input);

            memoryProfiler.Reset();

            Console.WriteLine("Matches: {0:#,##0}", matches.Count);

            decimal elapsed = ((decimal)stopwatch.ElapsedMilliseconds) / 1000;

            long deltaBefore = memoryProfiler.DeltaValue;
            memoryProfiler.CollectGC();
            long deltaAfter = memoryProfiler.DeltaValue;

            if (matches.Count > 0)
                Console.WriteLine("Last:    {0:#,##0} chars", matches.Last().Value.Length);

            Console.WriteLine("Memory:  {0,10:#,##0} bytes", deltaBefore);
            Console.WriteLine("AfterGC: {0,10:#,##0} bytes", deltaAfter);
            Console.WriteLine("Time:    {0:#0.000} sec.\n\n", elapsed);

            return matches;
        }
    }
}
