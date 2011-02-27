using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Asserts;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class AlternationMatcherTests_Backtr : AlgorithmTests
    {
        public AlternationMatcherTests_Backtr(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void MustBacktrack()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(a|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(ab|a)bbbc", AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(||ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(||)bbbc", AlgorithmType);

            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(|a)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(b*|a)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(ab+|a)bbbc", AlgorithmType);
        }
    }

#endif
}
