using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Patterns;

namespace RegexParser.Tests.Patterns
{
    [TestFixture]
    public class CharGroupPatternTests
    {
        [Test]
        public void Ranges()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z]");
            BasePattern expected = new GroupPattern(new BasePattern[] { new CharGroupPattern(true, new[] { uppercase }) });
            Assert.AreEqual(expected, actual, "One Range");


            actual = BasePattern.CreatePattern("[A-Z][a-z][0-9][a-zA-Z][A-Za-z0-9][a-zm-wA-M]");

            expected = new GroupPattern(new BasePattern[] {
                new CharGroupPattern(true, new[] { uppercase }),
                new CharGroupPattern(true, new[] { lowercase }),
                new CharGroupPattern(true, new[] { digits }),
                new CharGroupPattern(true, new[] { lowercase, uppercase }),
                new CharGroupPattern(true, new[] { uppercase, lowercase, digits }),
                new CharGroupPattern(true, new[] { lowercase,
                                                   new CharRangePattern('m', 'w'),
                                                   new CharRangePattern('A', 'M') })
            });

            Assert.AreEqual(expected, actual, "Many Ranges");
        }

        [Test]
        public void CharSetsAndRanges()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z]");

            BasePattern expected = new GroupPattern(new BasePattern[] { new CharGroupPattern(true, new[] { uppercase }) });
            Assert.AreEqual(expected, actual, "One Range");


            actual = BasePattern.CreatePattern("[.,;:?!A-Za-z][xya-zm-wA-M][msabcdm ;xyz]");

            expected = new GroupPattern(new BasePattern[] {
                new CharGroupPattern(true, ".,;:?!", new[] { uppercase, lowercase }),
                new CharGroupPattern(true, "xy", new[] { lowercase,
                                                         new CharRangePattern('m', 'w'),
                                                         new CharRangePattern('A', 'M') }),
                new CharGroupPattern(true, "msabcd ;xyz")
            });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NegativeRanges()
        {
            BasePattern actual = BasePattern.CreatePattern("[A-Z][^x][^a-z][0-9]");

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharGroupPattern(true, new[] { uppercase }),
                new CharGroupPattern(false, "x"),
                new CharGroupPattern(false, new[] { lowercase }),
                new CharGroupPattern(true, new[] { digits })
            });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Equality()
        {
            CharGroupPattern pattern1 = new CharGroupPattern(true, "xy");
            CharGroupPattern pattern2 = new CharGroupPattern(true, "xyxyy");
            Assert.AreEqual(pattern1, pattern2, "CharSet");

            pattern1 = new CharGroupPattern(true, new[] { uppercase, lowercase });
            pattern2 = new CharGroupPattern(true, new[] { uppercase, lowercase });
            Assert.AreEqual(pattern1, pattern2, "CharRange");

            pattern1 = new CharGroupPattern(true, "xyyx", new[] { uppercase, lowercase });
            pattern2 = new CharGroupPattern(true, "xxy", new[] { uppercase, lowercase });
            Assert.AreEqual(pattern1, pattern2, "CharSet/CharRange");

            Assert.AreEqual(pattern1.CharSet, pattern2.CharSet, "CharRange");
            Assert.AreEqual(pattern1.ChildPatterns[0], pattern2.ChildPatterns[0], "CharRange");

            pattern1 = new CharGroupPattern(true, new[] { uppercase, lowercase });
            pattern2 = new CharGroupPattern(true, new[] { uppercase, lowercase, digits });
            Assert.AreNotEqual(pattern1, pattern2, "NotEqual");
            Assert.IsTrue(pattern1 != pattern2, "!=");
        }

        private static CharRangePattern uppercase = new CharRangePattern('A', 'Z'),
                                        lowercase = new CharRangePattern('a', 'z'),
                                        digits = new CharRangePattern('0', '9');
    }
}
