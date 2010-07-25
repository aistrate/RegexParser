using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Patterns;

namespace RegexParser.Tests.Patterns
{
    [TestFixture]
    public class CharClassPatternTests
    {
        [Test]
        public void Ranges()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z]");
            BasePattern expected = new GroupPattern(new BasePattern[] { new CharClassPattern(true, new[] { uppercase }) });
            Assert.AreEqual(expected, actual, "One Range");


            actual = BasePattern.CreatePattern("[A-Z][a-z][0-9][a-zA-Z][A-Za-z0-9][a-zm-wA-M]");

            expected = new GroupPattern(new BasePattern[] {
                new CharClassPattern(true, new[] { uppercase }),
                new CharClassPattern(true, new[] { lowercase }),
                new CharClassPattern(true, new[] { digits }),
                new CharClassPattern(true, new[] { lowercase, uppercase }),
                new CharClassPattern(true, new[] { uppercase, lowercase, digits }),
                new CharClassPattern(true, new[] { lowercase,
                                                   new CharRangePattern('m', 'w'),
                                                   new CharRangePattern('A', 'M') })
            });

            Assert.AreEqual(expected, actual, "Many Ranges");
        }

        [Test]
        public void CharSetsAndRanges()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z]");

            BasePattern expected = new GroupPattern(new BasePattern[] { new CharClassPattern(true, new[] { uppercase }) });
            Assert.AreEqual(expected, actual, "One Range");


            actual = BasePattern.CreatePattern("[.,;:?!A-Za-z][xya-zm-wA-M][msabcdm ;xyz]");

            expected = new GroupPattern(new BasePattern[] {
                new CharClassPattern(true, ".,;:?!", new[] { uppercase, lowercase }),
                new CharClassPattern(true, "xy", new[] { lowercase,
                                                         new CharRangePattern('m', 'w'),
                                                         new CharRangePattern('A', 'M') }),
                new CharClassPattern(true, "msabcd ;xyz")
            });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NegativeRanges()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z][^x][^a-z][0-9]");

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharClassPattern(true, new[] { uppercase }),
                new CharClassPattern(false, "x"),
                new CharClassPattern(false, new[] { lowercase }),
                new CharClassPattern(true, new[] { digits })
            });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Equality()
        {
            CharClassPattern pattern1 = new CharClassPattern(true, "xy");
            CharClassPattern pattern2 = new CharClassPattern(true, "xyxyy");
            Assert.AreEqual(pattern1, pattern2, "CharSet");

            pattern1 = new CharClassPattern(true, new[] { uppercase, lowercase });
            pattern2 = new CharClassPattern(true, new[] { uppercase, lowercase });
            Assert.AreEqual(pattern1, pattern2, "CharRange");

            pattern1 = new CharClassPattern(true, "xyyx", new[] { uppercase, lowercase });
            pattern2 = new CharClassPattern(true, "xxy", new[] { uppercase, lowercase });
            Assert.AreEqual(pattern1, pattern2, "CharSet/CharRange");

            Assert.AreEqual(pattern1.ChildPatterns[0], pattern2.ChildPatterns[0], "CharEscape");
            Assert.AreEqual(pattern1.ChildPatterns[2], pattern2.ChildPatterns[2], "CharRange");

            pattern1 = new CharClassPattern(true, new[] { uppercase, lowercase });
            pattern2 = new CharClassPattern(true, new[] { uppercase, lowercase, digits });
            Assert.AreNotEqual(pattern1, pattern2, "NotEqual");
            Assert.IsTrue(pattern1 != pattern2, "!=");
        }

        private static CharRangePattern uppercase = new CharRangePattern('A', 'Z'),
                                        lowercase = new CharRangePattern('a', 'z'),
                                        digits = new CharRangePattern('0', '9');
    }
}
