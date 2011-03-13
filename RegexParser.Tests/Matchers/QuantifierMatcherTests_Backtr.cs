using System.Linq;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;
using Utility.BaseTypes;

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

        // TODO: add test: regex that decides whether a number is prime
    }

#endif
}
