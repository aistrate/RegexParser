using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RegexParser.Matchers;
using RegexParser.Patterns;
using RegexParser.Transforms;
using RegexParser.Util;
using UnitTesting;
using Utility.BaseTypes;
using Utility.ConsLists;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Asserts
{
    public static class RegexAssert
    {
        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType)
        {
            AreMatchesSameAsMsoft(input, pattern, algorithmType, RegexOptions.None);
        }

        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType, RegexOptions options)
        {
            //DisplayPattern(pattern);

            Match2[] actual = new Regex2(pattern, algorithmType, options).Matches(input).ToArray();

            DisplayMatches(input, pattern, algorithmType, options, actual);

            Match2[] expected = Msoft.Regex.Matches(input, pattern, ToMsoftRegexOptions(options))
                                           .Cast<Msoft.Match>()
                                           .Select(m => createMatch(m))
                                           .ToArray();

            try
            {
                CollectionAssert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(input, pattern, options, ex));
            }
        }

        public static void AreMatchesSameAsMsoft(string input, string[] patterns, AlgorithmType algorithmType)
        {
            AreMatchesSameAsMsoft(input, patterns, algorithmType, RegexOptions.None);
        }

        public static void AreMatchesSameAsMsoft(string input, string[] patterns, AlgorithmType algorithmType, RegexOptions options)
        {
            foreach (string pattern in patterns)
                AreMatchesSameAsMsoft(input, pattern, algorithmType, options);
        }

        public static void ThrowsSameExceptionAsMsoft(string input, string pattern, AlgorithmType algorithmType)
        {
            ThrowsSameExceptionAsMsoft(input, pattern, algorithmType, RegexOptions.None);
        }

        public static void ThrowsSameExceptionAsMsoft(string input, string pattern, AlgorithmType algorithmType, RegexOptions options)
        {
            Exception expected = catchException(() => { Msoft.Regex.Matches(input, pattern, ToMsoftRegexOptions(options)); },
                                                ".NET Regex", input, pattern, options),
                      actual = catchException(() => { new Regex2(pattern, algorithmType, options).Matches(input); },
                                              "Regex2", input, pattern, options);

            DisplayExpectedException(input, pattern, algorithmType, options, actual);

            try
            {
                CollectionAssert.AreEqual(exceptionChain(expected).Select(ex => ExceptionWrapper.Create(ex)).ToArray(),
                                          exceptionChain(actual).Select(ex => ExceptionWrapper.Create(ex)).ToArray(),
                                          "Comparing exceptions thrown.");
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(input, pattern, options, ex));
            }
        }

        public static void ThrowSameExceptionsAsMsoft(string input, string[] patterns, AlgorithmType algorithmType)
        {
            ThrowSameExceptionsAsMsoft(input, patterns, algorithmType, RegexOptions.None);
        }

        public static void ThrowSameExceptionsAsMsoft(string input, string[] patterns, AlgorithmType algorithmType, RegexOptions options)
        {
            foreach (string pattern in patterns)
                ThrowsSameExceptionAsMsoft(input, pattern, algorithmType, options);
        }

        public static Msoft.RegexOptions ToMsoftRegexOptions(RegexOptions options)
        {
            return (Msoft.RegexOptions)(int)options;
        }

        public static void DisplayPattern(string pattern)
        {
            var result = PatternParsers.Regex(new ArrayConsList<char>(pattern));

            Console.WriteLine("Pattern:\n    {0}", pattern.ShowVerbatim());
            Console.WriteLine("Parsed:");
            Console.WriteLine(result.Value.FormatAsTree(1));

            if (!result.Rest.IsEmpty)
                Console.WriteLine("Rest:\n    {0}", result.Rest.AsEnumerable().AsString().ShowVerbatim());

            Console.Write("\n");
        }

        public static void IsASTTransformCorrect(BasePattern expected, string patternText, BaseASTTransform transform)
        {
            BasePattern beforePattern = BasePattern.CreatePattern(patternText);
            BasePattern afterPattern = doTransform(beforePattern, transform);

            displayASTTransform(patternText, beforePattern, afterPattern);

            Assert.AreEqual(expected, afterPattern);
        }

        public static void IsASTTransformCorrect(BasePattern expected, string patternText, AlgorithmType algorithmType, RegexOptions options)
        {
            BasePattern beforePattern = BasePattern.CreatePattern(patternText);
            BasePattern afterPattern = doTransform(patternText, algorithmType, options);

            displayASTTransform(patternText, beforePattern, afterPattern);

            Assert.AreEqual(expected, afterPattern);
        }

        public static void DisplayASTTransform(string patternText, BaseASTTransform transform)
        {
            BasePattern beforePattern = BasePattern.CreatePattern(patternText);
            BasePattern afterPattern = doTransform(beforePattern, transform);

            displayASTTransform(patternText, beforePattern, afterPattern);
        }

        public static void DisplayASTTransform(string patternText, AlgorithmType algorithmType, RegexOptions options)
        {
            BasePattern beforePattern = BasePattern.CreatePattern(patternText);
            BasePattern afterPattern = doTransform(patternText, algorithmType, options);

            displayASTTransform(patternText, beforePattern, afterPattern);
        }

        private static void displayASTTransform(string patternText, BasePattern beforePattern, BasePattern afterPattern)
        {
            Console.WriteLine("Pattern Text:\n    {0}", patternText.ShowVerbatim());
            Console.WriteLine("Before Transform:");
            Console.WriteLine(beforePattern.FormatAsTree(1));
            Console.WriteLine("After Transform:");
            Console.WriteLine(afterPattern.FormatAsTree(1));

            Console.Write("\n");
        }

        private static BasePattern doTransform(BasePattern pattern, BaseASTTransform transform)
        {
            return transform.Transform(pattern);
        }

        private static BasePattern doTransform(string patternText, AlgorithmType algorithmType, RegexOptions options)
        {
            return BaseMatcher.CreateMatcher(algorithmType, patternText, options).Pattern;
        }

        public static void DisplayMatches(string input, string pattern, AlgorithmType algorithmType, RegexOptions options,
                                          IEnumerable<Match2> matches)
        {
            displayHeader(input, pattern, options);

            int count = matches.Count();
            Console.WriteLine("{0} match{1} ({2}){3}",
                              count,
                              count == 1 ? "" : "es",
                              algorithmType.ToString(),
                              count > 0 ? ":" : ".");

            if (count > 0)
                Console.WriteLine(matches.Select(m => string.Format("{0,4:#0},{1,3:#0},  {2}",
                                                                    m.Index, m.Length, m.Value.Show()))
                                         .JoinStrings("\n"));

            Console.Write("\n");
        }

        public static void DisplayExpectedException(string input, string pattern, AlgorithmType algorithmType, RegexOptions options,
                                                    Exception ex)
        {
            displayHeader(input, pattern, options);

            Console.WriteLine("Exception was expected, this was thrown ({0}):", algorithmType.ToString());

            const string indent = "   ";

            for (Exception e = ex; e != null; e = e.InnerException)
            {
                Console.WriteLine(indent + "[{0}]", e.GetType().FullName);
                Console.WriteLine(indent + indent + e.Message.Replace("\n", "\n" + indent + indent));
            }

            Console.Write("\n");
        }

        private static void displayHeader(string input, string pattern, RegexOptions options)
        {
            Console.WriteLine("Input:    {0}", input.Show());
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            if (options != RegexOptions.None)
                Console.WriteLine("Options: [{0}]", options.ToString());
        }

        private static Match2 createMatch(Msoft.Match msoftMatch)
        {
            if (msoftMatch.Success)
                return Factory.CreateMatch(msoftMatch.Index, msoftMatch.Length, msoftMatch.Value);
            else
                return Match2.Empty;
        }

        private static string formatException(string input, string pattern, RegexOptions options, Exception ex)
        {
            const string indent = "  ";

            string message = string.Format("Comparing with .NET Regex: Input={0}, Pattern={1}{2}\n",
                                           input.Show(),
                                           pattern.ShowVerbatim(),
                                           options != RegexOptions.None ?
                                                string.Format(", Options=[{0}]", options.ToString()) :
                                                "") +
                             ex.Message;
            message = indent + message.Replace("\n", "\n" + indent);

            return message;
        }

        private static Exception catchException(Action runRegex, string regexName, string input, string pattern, RegexOptions options)
        {
            Exception caughtException = null;

            try
            {
                runRegex();
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            if (caughtException != null)
                return caughtException;
            else
                throw new AssertionException(formatException(input, pattern, options,
                      new AssertionException(regexName + " did not throw an exception, though was supposed to.")));
        }

        private static IEnumerable<Exception> exceptionChain(Exception ex)
        {
            for (Exception e = ex; e != null; e = e.InnerException)
                yield return e;
        }
    }
}
