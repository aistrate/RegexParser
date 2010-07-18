using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Matchers
{
    [TestFixture(AlgorithmType.ImplicitDFA)]
    public class QuantifierMatcherTests : MatcherTests
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

        [Test]
        public void Doubled_Error()
        {
            RegexAssert.ThrowsSameExceptionAsMsoft("abab", @"(ab)++", AlgorithmType);
        }

        //[Test]
        public void ZeroWidth()
        {
            RegexAssert.AreMatchesSameAsMsoft("bbbcc", @"b*", AlgorithmType);
        }
    }
}
