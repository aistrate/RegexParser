using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Patterns;

namespace RegexParser.Tests.Patterns
{
    [TestFixture]
    public class PatternTests
    {
        [Test]
        public void CharPattern_Short()
        {
            BasePattern actual = BasePattern.CreatePattern("abc");

            BasePattern expected = new GroupPattern(new CharPattern[] {
                new CharPattern('a'), 
                new CharPattern('b'),
                new CharPattern('c')
            });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CharPattern_Empty()
        {
            BasePattern actual = BasePattern.CreatePattern("");

            BasePattern expected = new GroupPattern(new CharPattern[] { });

            Assert.AreEqual(expected, actual);
        }
    }
}
