using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
#if TEST_BACKTRACKING
    [TestFixture(AlgorithmType.Backtracking)]
#endif

#if TEST_BACKTRACKING

    public class LazyQuantifierMatcherTests : AlgorithmTests
    {
        public LazyQuantifierMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Many()
        {
            string input = "A thing or  another thing ";

            string[] patterns = new[] {
                @"\w*?",
                @"\w+?",
                @"\s+?",
                @"\w\w*?",
                @"\w\s*?",
                @"\w\s+?",
                @"\w\s*?\s",
                @"\w\s+?\s",
                @"\w\s*?\w",
                @"\w\s+?\w",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ZeroOrOne()
        {
            string input = "abbcccddddeeeee 122333444455555666666";

            string[] patterns = new[] {
                @"cd??ddde",
                @"c??d??ddde",
                @"c??d??ddd",
                @"c??ddd",
                @"c??d?ddd",
                @"c?d?ddd",
                @"c?d??ddd",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void ZeroOrOne_LongChain()
        {
            string input = "123456";

            string[] patterns = new[] {
                @"2??3??4??5?",
                @"2??3??4??5",
                @"2?3??4??5?",
                @"2?3??4??5??",
                @"2??3??4??5??",
                @"2??3??4??5+?",
                @"2?3?4?5?",

                @"8??3??4??5",
                @"8?3?4?5",
                @"8??1??2??3",
                @"8?1?2?3",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void WebSiteAddresses()
        {
            string input = " www.microsoft.com  msdn.microsoft.com  mywebsite  mycompany.com ";

            string[] patterns = new[] {
                @" (\w{3,}?\.){2}?\w{3,}? ",
                @" (\w{3,}?\.){2}\w{3,}? ",
                @" (\w{3,}\.){2}\w{3,} ",

                @" (\w{3,}?\.){1}?\w{3,}? ",
                @" (\w{3,}?\.){1,}?\w{3,}? ",
                @" (\w{3,}?\.){1,}\w{3,}? ",
                @" (\w{3,}\.){1,}\w{3,} ",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void FromNToM()
        {
            string input = "abbcccddddeeeee 122333444455555666666";

            string[] patterns = new[] {
                @"c{1}?",
                @"c{3}?",
                @"c{1,}?",
                @"c{2,}?",
                @"c{1,3}?",
                @"c{2,4}?",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void Combined()
        {
            string input = "abbcccddddeeeee 122333444455555666666";

            string[] patterns = new[] {
                @"c{2,4}?d{1,}?e+?\s*?\d{5}",
                @"c{2,4}?d{1,}?e+?",
                @"c{2,4}?d{1,}?e*?",
                @"c{2,4}?d{1,}?e*",

                @"a[bcd]+?",
                @"a[bcd]+?e",
                @"a[bcd]+?e+\s",
                @"a[b-e\d\s]+?6",
                @"a[b-e\d\s]+?6+",
                @"a[b-e\d\s]+?6*",
                @"a[b-e\d\s]+?6+?",
                @"a[b-e\d\s]+?6{1,}?",
                @"a[b-e\d\s]+?6{1}?",
            };

            RegexAssert.AreMatchesSameAsMsoft(input, patterns, AlgorithmType);
        }

        [Test]
        public void MustBacktrack()
        {
            RegexAssert.AreMatchesSameAsMsoft("aaac", @"a*?ac", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("aaax", @"[ab]*?\wx", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("aaax", @"\S*?\wx", AlgorithmType);
        }
    }

#endif
}
