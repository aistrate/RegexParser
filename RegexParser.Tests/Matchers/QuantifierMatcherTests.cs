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

    public class QuantifierMatcherTests : AlgorithmTests
    {
        public QuantifierMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Simple()
        {
            string input = "A thing or another thing ";

            string[] patterns = new[] {
                @"\w+",
                @"\s+",
                @"\w\w*",
                @"\s\w*"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void Nested()
        {
            // TODO: create similar tests that need backtracking
            RegexAssert.AreMatchesSameAsMsoft("abab", @"(aba?)+", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbab", @"(aba?)+", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbab", @"(abb?)+", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("abbab", @"abb?", AlgorithmType);
        }

        [Test]
        public void Doubled()
        {
            RegexAssert.AreMatchesSameAsMsoft("abab", @"((ab)+)+", AlgorithmType);
        }

        //[Test]
        public void Doubled_Error()
        {
            RegexAssert.ThrowsSameExceptionAsMsoft("abab", @"(ab)++", AlgorithmType);
        }

        [Test]
        public void ZeroWidth()
        {
            RegexAssert.AreMatchesSameAsMsoft("bbbcc", @"b*", AlgorithmType);
        }

        [Test]
        public void FromNToM()
        {
            string input = "abbcccddddeeeee 122333444455555666666";

            string[] patterns = new[] {
                @"c{1,2}",
                @"c{1,3}",
                @"c{2,5}",
                @"c{3,7}",
                @"c{4,5}",
                @"c{3,3}",
                @"c{1,1000000}",

                @"e{4,5}",
                @"e{5,6}",
                @"e{4,6}",
                @"e{5,5}",

                @"1{1,1}",
                @"1{1,}",
                @"1{2,}",
                @"1{1}",
                @"1{2}",

                @"d{4}",
                @"d{3}",
                @"d{5}",
                @"d{4,}",
                @"d{3,}",
                @"d{1,}",

                @"e{5}",

                @"\d{1,}",
                @"\d{3,}",
                @"\d{21,}",
                @"\d{22,}",

                @"\w{1,10}",
                @"\w{1,15}",
                @"\w{1,16}",

                @"\w{1,}",
                @"\w{10,}",
                @"\w{15,}",
                @"\w{16,}",
                @"\w{20,}",

                @"[0-9a-z ]{1,}",
                @"[0-9a-z ]{22,}",
                @"[0-9a-z]{1,}"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void FromNToM_ZeroWidth()
        {
            string input = "abbcccddddeeeee 122333444455555666666";

            string[] patterns = new[] {
                @"c{0,1000000}",

                @"1{0,0}",
                @"1{0,1}",

                @"1{0}",
                @"1{0,}",

                @"d{0,}",

                @"\d{0,}",

                @"\w{0,10}",
                @"\w{0,15}",
                @"\w{0,16}",

                @"\w{0,}",

                @"[0-9a-z ]{0,}",
                @"[0-9a-z]{0,}"
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }
    }

#endif
}
