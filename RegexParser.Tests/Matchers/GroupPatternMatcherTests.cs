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
    public class GroupPatternMatcherTests : MatcherTests
    {
        public GroupPatternMatcherTests(AlgorithmType algorithmType)
            : base(algorithmType) { }

        [Test]
        public void Grouping()
        {
            string input = "A thing or another thing";

            RegexAssert.AreMatchesSameAsMsoft(input, "th(in)g", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "(thing)", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "t(hi)n(g)", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "t(h(i)n)g", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft(input, "th(((((in)))))g", AlgorithmType);
        }

        [Test]
        public void FalseStart()
        {
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "Som(me)", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "So(mme)", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "So(m)", AlgorithmType);
            RegexAssert.AreMatchesSameAsMsoft("Something or other", "So(m)e", AlgorithmType);
        }

        //[Test]
        //public void Grouping_ErrorHandling()
        //{
        //    string input = "A thing or another thing";

        //    Assert.Catch<ArgumentException>(() => { new Regex2("th((in)g", AlgorithmType).Match(input); });
        //    Assert.Catch<ArgumentException>(() => { new Regex2("th((in)))g", AlgorithmType).Match(input); });
        //    Assert.Catch<ArgumentException>(() => { new Regex2("(t(h(in))g", AlgorithmType).Match(input); });
        //    Assert.Catch<ArgumentException>(() => { new Regex2("thing)))", AlgorithmType).Match(input); });
        //    Assert.Catch<ArgumentException>(() => { new Regex2("(thing", AlgorithmType).Match(input); });
        //    Assert.Catch<ArgumentException>(() => { new Regex2(")thing", AlgorithmType).Match(input); });
        //    Assert.Catch<ArgumentException>(() => { new Regex2("thi)ng", AlgorithmType).Match(input); });
        //}
    }
}
