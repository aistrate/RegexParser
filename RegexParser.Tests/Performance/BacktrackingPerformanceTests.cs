using System;
using System.Diagnostics;
using System.Linq;
using RegexParser.Matchers;
using Utility.BaseTypes;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Performance
{
    public static class BacktrackingPerformanceTests
    {
        public static void BacktrackingTest()
        {
            const int n = 20;

            string input = new string('a', n);

            string pattern = Enumerable.Repeat("a?", n).JoinStrings() + input;


            Console.WriteLine("Input:    {0}", input.Show());
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            displayTest("", () => countMatches(input, pattern));
            //  7:  0.245 sec.
            // 10:  0.273 sec.
            // 15:  0.381 sec.
            // 19:  1.921 sec.
            // 20:  3.694 sec.
            // 21:  7.385 sec.

            displayTest("Microsoft:", () => countMatches_Msoft(input, pattern));
            //  7:  0.002 sec.
            // 10:  0.002 sec.
            // 15:  0.005 sec.
            // 19:  0.052 sec.
            // 20:  0.103 sec.
            // 21:  0.206 sec.

            // 25:  3.315 sec.
            // 29: 52.146 sec.
        }

        private static void displayTest(string title, Func<int> getMatchCount)
        {
            Console.WriteLine();

            if (!string.IsNullOrEmpty(title))
                Console.WriteLine(title);

            Stopwatch stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Matches: {0:#,##0}", getMatchCount());

            decimal elapsed = ((decimal)stopwatch.ElapsedMilliseconds) / 1000;
            Console.WriteLine("Time:    {0:#0.000} sec.", elapsed);
        }

        private static int countMatches(string input, string pattern)
        {
            return new Regex2(pattern, AlgorithmType.Backtracking).Matches(input).Count;
        }

        private static int countMatches_Msoft(string input, string pattern)
        {
            return new Msoft.Regex(pattern).Matches(input).Count;
        }
    }
}
