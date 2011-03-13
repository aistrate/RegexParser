using System;
using System.Linq;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;
using Utility.BaseTypes;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class QuantifierMatcherTests_Backtr : AlgorithmTests
    {
        public QuantifierMatcherTests_Backtr(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void MustBacktrack()
        {
            RegexAssert.AreMatchesSameAsMsoft("aaac", @"a*ac", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("aaax", @"[ab]*\wx", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("aaax", @"\S*\wx", AlgorithmType);
        }

        [Test]
        public void ExponentialTime()
        {
            // TODO: run this with ExplicitDFAMatcher (when fully working)
            const int n = 7;

            string input = new string('a', n);

            string pattern = Enumerable.Repeat("a?", n).JoinStrings() + input;

            RegexAssert.AreMatchesSameAsMsoft(input, pattern, AlgorithmType);
        }

        // TODO: enable this test when captures work properly
        //[Test]
        public void PrimeNumbers()
        {
            int[] numbers = new[]
                {
                    0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                    10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                    20, 23, 24, 25, 26, 29,
                    31, 37,
                    41, 43, 47,
                    53, 59,
                    60, 61, 64, 67,
                    71, 72, 73,
                };

            string pattern = @"^1?$|^(11+?)\1+$";

            foreach (int n in numbers)
            {
                string input = new string('1', n);

                Console.WriteLine("Number:   {0}", n);
                Console.WriteLine("Prime:    {0}", Msoft.Regex.Matches(input, pattern).Count == 0);

                RegexAssert.AreMatchesSameAsMsoft(input, pattern, AlgorithmType);
                //RegexAssert.DisplayMsoftMatches(input, pattern);
            }
        }
    }

#endif
}
