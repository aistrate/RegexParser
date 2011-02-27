using System;
using System.Diagnostics;
using RegexParser.Matchers;
using Utility.BaseTypes;

namespace RegexParser.Tests.Performance
{
    public static class TransformPerformanceTests
    {
        public static void TransformTests()
        {
            const int times = 10000;

            createRegex(@"(\w{1000})+", times);
            //  2.360 sec. (no transform)
            //  3.118 sec. (Identity transform)
            //  2.407 sec. (ExplicitDFA, no ReferenceEquals)
            //  2.511 sec. (ExplicitDFA)
            //  2.310 sec. (Backtracking, no ReferenceEquals)
            //  2.910 sec. (Backtracking)

            createRegex(@"(abcdefghijklmnopqrstuvwxyz)+", times);
            // 22.135 sec. (no transform)
            // 22.136 sec. (Identity transform)
            // 21.559 sec. (Identity transform, no ReferenceEquals)
            // 22.206 sec. (ExplicitDFA)
            // 22.144 sec. (Backtracking)
            // 21.755 sec. (Backtracking, no ReferenceEquals)

            createRegex(@"([ab]*cd?|[xyz]+mn?p?(stu)+)?|aaa*|a?b?cd+", times);
            //  7.008 sec. (no transform)
            // 20.119 sec. (Identity transform)
            //  7.274 sec. (Identity transform, no ReferenceEquals)
            //  8.840 sec. (ExplicitDFA)
            //  7.337 sec. (ExplicitDFA, no ReferenceEquals)
            // 16.501 sec. (Backtracking)
            //  7.295 sec. (Backtracking, no ReferenceEquals)
        }

        private static void createRegex(string pattern, int times)
        {
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            Stopwatch stopwatch = Stopwatch.StartNew();

            Regex2 regex;
            for (int i = 0; i < times; i++)
                regex = new Regex2(pattern, AlgorithmType.Backtracking);

            decimal elapsed = ((decimal)stopwatch.ElapsedMilliseconds) / 1000;

            Console.WriteLine("Time:    {0:#0.000} sec.\n", elapsed);
        }
    }
}
