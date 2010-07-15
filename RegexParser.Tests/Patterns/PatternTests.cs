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

        [Test]
        public void CharClassPattern()
        {
            BasePattern actual = BasePattern.CreatePattern(
                                    "[A-Z][a-z][0-9][A-Za-z][A-Za-z0-9][A-Z.,;:?!a-z][a-zxym-wA-M][abcdms ;xyz][^x][^a-z]");

            BasePattern expected = new GroupPattern(new BasePattern[] {
                new CharClassPattern(true, new[] { uppercase }),
                new CharClassPattern(true, new[] { lowercase }),
                new CharClassPattern(true, new[] { digits }),
                new CharClassPattern(true, new[] { lowercase, uppercase }),
                new CharClassPattern(true, new[] { digits, uppercase, lowercase }),
                new CharClassPattern(true, ".,;:?!", new[] { uppercase, lowercase }),
                new CharClassPattern(true, "xy", new[] { lowercase,
                                                         new CharClassPattern.CharRange('m', 'w'),
                                                         new CharClassPattern.CharRange('A', 'M') }),
                new CharClassPattern(true, "abcdms ;xyz"),
                
                new CharClassPattern(false, "x"),
                new CharClassPattern(false, new[] { lowercase })
            });

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CharClassPattern_Equality()
        {
            CharClassPattern pattern1 = new CharClassPattern(true, "xy");
            CharClassPattern pattern2 = new CharClassPattern(true, "yxy");
            Assert.AreEqual(pattern1, pattern2, "CharSet");
            Assert.IsTrue(pattern1 == pattern2, "CharSet/==");

            pattern1 = new CharClassPattern(true, new[] { lowercase, uppercase });
            pattern2 = new CharClassPattern(true, new[] { uppercase, lowercase });
            Assert.AreEqual(pattern1, pattern2, "CharRange");
            Assert.IsTrue(pattern1 == pattern2, "CharRange/==");

            pattern1 = new CharClassPattern(true, "xy", new[] { lowercase, uppercase });
            pattern2 = new CharClassPattern(true, "yxy", new[] { uppercase, lowercase });
            Assert.AreEqual(pattern1, pattern2, "CharSet/CharRange");
            Assert.IsTrue(pattern1 == pattern2, "CharSet/CharRange/==");

            Assert.AreEqual(pattern1.CharRanges[0], pattern2.CharRanges[0], "CharRange");
            Assert.IsTrue(pattern1.CharRanges[0] == pattern2.CharRanges[0], "CharRange/==");

            
            pattern1 = new CharClassPattern(true, new[] { uppercase, lowercase });
            pattern2 = new CharClassPattern(true, new[] { uppercase, lowercase, digits });
            Assert.AreNotEqual(pattern1, pattern2, "NotEqual");
            Assert.IsTrue(pattern1 != pattern2, "!=");
        }

        private static CharClassPattern.CharRange uppercase = new CharClassPattern.CharRange('A', 'Z'),
                                                  lowercase = new CharClassPattern.CharRange('a', 'z'),
                                                  digits    = new CharClassPattern.CharRange('0', '9');

        private static string range(char from, char to)
        {
            StringBuilder sb = new StringBuilder();

            for (char c = from; c <= to; c++)
                sb.Append(c);

            return sb.ToString();
        }
    }
}
