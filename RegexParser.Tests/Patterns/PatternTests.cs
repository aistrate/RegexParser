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

            BasePattern expected = new GroupPattern(new BasePattern[] {
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

            BasePattern expected = new GroupPattern(new BasePattern[] { });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GroupPattern()
        {
            string pattern = "t(h(in))g";

            BasePattern actual = BasePattern.CreatePattern(pattern);

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharPattern('t'),
                new GroupPattern(new BasePattern[] {
                    new CharPattern('h'),
                    new GroupPattern(new BasePattern[] {
                        new CharPattern('i'),
                        new CharPattern('n')
                    }),
                }),
                new CharPattern('g')
            });

            Assert.AreEqual(expected, actual, pattern);
        }
    }
}
