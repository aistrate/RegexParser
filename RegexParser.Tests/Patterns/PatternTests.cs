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
        public void CharEscapePattern_Short()
        {
            BasePattern actual = BasePattern.CreatePattern("abc");

            BasePattern expected = new GroupPattern(true, new BasePattern[] {
                new CharEscapePattern('a'),
                new CharEscapePattern('b'),
                new CharEscapePattern('c')
            });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CharEscapePattern_Empty()
        {
            BasePattern actual = BasePattern.CreatePattern("");

            BasePattern expected = new GroupPattern(true, new BasePattern[] { });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GroupPattern()
        {
            string pattern = "t(h(in))g";

            BasePattern actual = BasePattern.CreatePattern(pattern);

            BasePattern expected = new GroupPattern(true, new BasePattern[] {
                new CharEscapePattern('t'),
                new GroupPattern(true, new BasePattern[] {
                    new CharEscapePattern('h'),
                    new GroupPattern(true, new BasePattern[] {
                        new CharEscapePattern('i'),
                        new CharEscapePattern('n')
                    }),
                }),
                new CharEscapePattern('g')
            });

            Assert.AreEqual(expected, actual, pattern);
        }
    }
}
