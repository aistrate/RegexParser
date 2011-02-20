using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
#if TEST_EXPLICITDFA
    [TestFixture(AlgorithmType.ExplicitDFA)]
#endif
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_EXPLICITDFA || TEST_BACKTRACKING

    public class AlternationMatcherTests : AlgorithmTests
    {
        public AlternationMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Simple()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbc", @"(a|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(ab|a)bbbc", AlgorithmType);
        }

        [Test]
        public void FirstDoesNotMatch()
        {
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(c|ba|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(aa|ab)bbbc", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbbbc", @"(ba|aa|ab|a)bbbc", AlgorithmType);
        }

        [Test]
        public void NoParens()
        {
            RegexAssert.AreMatchesSameAsMsoft("abc", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("def", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abcdef", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abc def", @"abc|def", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abcadef", @"abc|def", AlgorithmType);
        }
    }

#endif
}
