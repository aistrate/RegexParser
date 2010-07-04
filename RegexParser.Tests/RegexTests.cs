using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Util;
using RegexParser.Tests.Util;

namespace RegexParser.Tests
{
    [TestFixture]
    public class RegexTests
    {
        [Test]
        public void CharPatternMatch()
        {
            Regex2 regex = new Regex2("thing");
            Match2 match = regex.Match("Something or other");

            Assert.AreEqual(Factory.CreateMatch(4, 5, "thing", null), match);
        }
    }
}
