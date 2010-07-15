using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.Tests.Performance;
using RegexParser.Patterns;

namespace RegexParser.Tests.Performance
{
    public static class PatternPerformanceTests
    {
        public static void CharClassPatternTest()
        {
            const int times = 200, maxItemCount = 100000;


            string digitChars = new string(EnumerablePerformanceTests.RepeatChars("0123456789", maxItemCount).ToArray());

            charClassPatternTest(new CharClassPattern(true, "0123456789"),
                                 times, maxItemCount, digitChars);
            // 6.33 sec.

            charClassPatternTest(new CharClassPattern(true, new[] { new CharClassPattern.CharRange('0', '9') }),
                                 times, maxItemCount, digitChars);
            // 2.91 sec.


            string lowercaseChars = new string(EnumerablePerformanceTests.RepeatChars("abcdefghijklmnopqrstuvwxyz", maxItemCount).ToArray());

            charClassPatternTest(new CharClassPattern(true, "abcdefghijklmnopqrstuvwxyz"),
                                 times, maxItemCount, lowercaseChars);
            // 9.05 sec.

            charClassPatternTest(new CharClassPattern(true, new[] { new CharClassPattern.CharRange('a', 'z') }),
                                 times, maxItemCount, lowercaseChars);
            // 2.95 sec.


            charClassPatternTest(new CharClassPattern(true, "0123456789abcdefghijklmnopqrstuvwxyz"),
                                 times, maxItemCount, lowercaseChars);
            // 12.33 sec.

            charClassPatternTest(new CharClassPattern(true, new[] { new CharClassPattern.CharRange('0', '9'),
                                                                    new CharClassPattern.CharRange('a', 'z') }),
                                 times, maxItemCount, lowercaseChars);
            // 3.19 sec.


            // Negative character classes
            charClassPatternTest(new CharClassPattern(false, "abcdefghijklmnopqrstuvwxyz"),
                                 times, maxItemCount, digitChars);
            // 12.86 sec.

            charClassPatternTest(new CharClassPattern(false, new[] { new CharClassPattern.CharRange('a', 'z') }),
                                 times, maxItemCount, digitChars);
            // 2.86 sec.
        }

        private static void charClassPatternTest(CharClassPattern charClassPattern, int times, int maxItemCount, string inputText)
        {
            Console.WriteLine(charClassPattern);
            CharParsersPerformanceTests.CharParserTest(CharParsers.Satisfy(c => charClassPattern.IsMatch(c)),
                                                       times, maxItemCount, inputText);

            Console.WriteLine();
        }
    }
}
