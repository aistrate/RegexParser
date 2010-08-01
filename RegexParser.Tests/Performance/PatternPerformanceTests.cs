using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators;
using ParserCombinators.Util;
using ParserCombinators.ConsLists;
using ParserCombinators.Tests.Performance;
using RegexParser.Patterns;

namespace RegexParser.Tests.Performance
{
    public static class PatternPerformanceTests
    {
        public static void CharClassPatternTest()
        {
            const int times = 200, maxItemCount = 100000;


            string digitChars = EnumerablePerformanceTests.RepeatChars("0123456789", maxItemCount).AsString();

            charTest("always true", c => true,
                     times, maxItemCount, digitChars);
            // 1.28 sec.

            charTest("char.IsDigit", c => char.IsDigit(c),
                     times, maxItemCount, digitChars);
            // 1.38 sec.

            charClassPatternTest(new CharGroupPattern(true, "0123456789"),
                                 times, maxItemCount, digitChars);
            // 2.39 sec.

            charClassPatternTest(new CharGroupPattern(true, new[] { new CharRangePattern('0', '9') }),
                                 times, maxItemCount, digitChars);
            // 1.59 sec.

            charClassPatternTest(parseCharClass(@"\d"),
                                 times, maxItemCount, digitChars);
            // 1.80 sec.

            charClassPatternTest(parseCharClass(@"[\d]"),
                                 times, maxItemCount, digitChars);
            // 2.19 sec.


            string lowercaseChars = EnumerablePerformanceTests.RepeatChars("abcdefghijklmnopqrstuvwxyz", maxItemCount).AsString();

            charTest("char.IsLetter", c => char.IsLetter(c),
                     times, maxItemCount, lowercaseChars);
            // 1.42 sec.

            charClassPatternTest(new CharGroupPattern(true, "abcdefghijklmnopqrstuvwxyz"),
                                 times, maxItemCount, lowercaseChars);
            // 2.55 sec.

            charClassPatternTest(new CharGroupPattern(true, new[] { new CharRangePattern('a', 'z') }),
                                 times, maxItemCount, lowercaseChars);
            // 1.65 sec.

            charClassPatternTest(parseCharClass(@"\w"),
                                 times, maxItemCount, digitChars);
            // 2.06 sec.

            charClassPatternTest(parseCharClass(@"[\w]"),
                                 times, maxItemCount, digitChars);
            // 2.42 sec.

            charClassPatternTest(parseCharClass(@"[\s\x00-\x1F\d\w]"),
                                 times, maxItemCount, digitChars);
            // 3.05 sec.


            charTest("char.IsLetterOrDigit", c => char.IsLetterOrDigit(c),
                     times, maxItemCount, lowercaseChars);
            // 1.64 sec.

            charClassPatternTest(new CharGroupPattern(true, "0123456789abcdefghijklmnopqrstuvwxyz"),
                                 times, maxItemCount, lowercaseChars);
            // 2.80 sec.

            charClassPatternTest(new CharGroupPattern(true, new[] { new CharRangePattern('0', '9'),
                                                                    new CharRangePattern('a', 'z') }),
                                 times, maxItemCount, lowercaseChars);
            // 1.69 sec.

            charClassPatternTest(parseCharClass(@"\w"),
                                 times, maxItemCount, digitChars);
            // 2.02 sec.

            charClassPatternTest(parseCharClass(@"[\w-[A-Z]]"),
                                 times, maxItemCount, digitChars);
            // 3.42 sec.


            string repeatedChar = EnumerablePerformanceTests.RepeatChars("7", maxItemCount).AsString();

            charClassPatternTest(new CharGroupPattern(true, "7"),
                                 times, maxItemCount, repeatedChar);
            // 1.97 sec.

            charClassPatternTest(new CharGroupPattern(true, new[] { new CharRangePattern('7', '7') }),
                                 times, maxItemCount, repeatedChar);
            // 1.61 sec.


            // Negative character classes
            charClassPatternTest(new CharGroupPattern(false, "abcdefghijklmnopqrstuvwxyz"),
                                 times, maxItemCount, digitChars);
            // 2.84 sec.

            charClassPatternTest(new CharGroupPattern(false, new[] { new CharRangePattern('a', 'z') }),
                                 times, maxItemCount, digitChars);
            // 1.64 sec.

            charClassPatternTest(parseCharClass(@"[^a-z]"),
                                 times, maxItemCount, digitChars);
            // 2.09 sec.
        }

        private static CharClassPattern parseCharClass(string pattern)
        {
            return PatternParsers.CharClass(new ArrayConsList<char>(pattern)).First().Value;
        }

        private static void charClassPatternTest(CharClassPattern charClassPattern, int times, int maxItemCount, string inputText)
        {
            charTest(charClassPattern.ToString(), c => charClassPattern.IsMatch(c), times, maxItemCount, inputText);
        }

        private static void charTest(string testName, Func<char, bool> predicate, int times, int maxItemCount, string inputText)
        {
            Console.WriteLine(testName);
            CharParsersPerformanceTests.CharParserTest(CharParsers.Satisfy(predicate),
                                                       times, maxItemCount, inputText);

            Console.WriteLine();
        }
    }
}
