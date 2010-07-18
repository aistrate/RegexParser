using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ParserCombinators.Util;
using RegexParser.Matchers;
using RegexParser.Util;
using Msoft = System.Text.RegularExpressions;

namespace RegexParser.Tests.Helpers
{
    public static class RegexAssert
    {
        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType)
        {
            AreMatchesSameAsMsoft(input, pattern, algorithmType, null);
        }

        public static void AreMatchesSameAsMsoft(string input, string pattern, AlgorithmType algorithmType, string message)
        {
            Match2[] actual = new Regex2(pattern, algorithmType).Matches(input).ToArray();

            DisplayMatches(input, pattern, algorithmType, actual);

            Match2[] expected = Msoft.Regex.Matches(input, pattern)
                                           .Cast<Msoft.Match>()
                                           .Select(m => createMatch(m))
                                           .ToArray();

            try
            {
                CollectionAssert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(message, input, pattern, ex));
            }
        }

        public static void AreMatchesSameAsMsoft(string input, string[] patterns, AlgorithmType algorithmType)
        {
            foreach (string pattern in patterns)
                AreMatchesSameAsMsoft(input, pattern, algorithmType);
        }

        public static void ThrowsSameExceptionAsMsoft(string input, string pattern, AlgorithmType algorithmType)
        {
            ThrowsSameExceptionAsMsoft(input, pattern, algorithmType, null);
        }

        public static void ThrowsSameExceptionAsMsoft(string input, string pattern, AlgorithmType algorithmType, string message)
        {
            Exception expected = catchException(() => { Msoft.Regex.Matches(input, pattern); },
                                                ".NET Regex", input, pattern, message),
                      actual = catchException(() => { new Regex2(pattern, algorithmType).Matches(input); },
                                              "Regex2", input, pattern, message);

            DisplayExpectedException(input, pattern, algorithmType, actual);

            try
            {
                CollectionAssert.AreEqual(exceptionChain(expected).Select(ex => ExceptionWrapper.Create(ex)).ToArray(),
                                          exceptionChain(actual).Select(ex => ExceptionWrapper.Create(ex)).ToArray(),
                                          "Comparing exceptions thrown.");
            }
            catch (Exception ex)
            {
                throw new AssertionException(formatException(message, input, pattern, ex));
            }
        }

        public static void ThrowSameExceptionsAsMsoft(string input, string[] patterns, AlgorithmType algorithmType)
        {
            foreach (string pattern in patterns)
                ThrowsSameExceptionAsMsoft(input, pattern, algorithmType);
        }

        public static void DisplayMatches(string input, string pattern, AlgorithmType algorithmType, IEnumerable<Match2> matches)
        {
            Console.WriteLine("Input: {0}", input.Show());
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            int count = matches.Count();
            Console.WriteLine("{0} match{1} ({2}){3}",
                              count,
                              count == 1 ? "" : "es",
                              algorithmType.ToString(),
                              count > 0 ? ":" : ".\n");

            if (count > 0)
                Console.WriteLine(string.Join("\n", matches.Select(m => string.Format("{0,4:#0},{1,3:#0},  {2}",
                                                                                      m.Index, m.Length, m.Value.Show()))
                                                           .ToArray()) + "\n");
        }

        public static void DisplayExpectedException(string input, string pattern, AlgorithmType algorithmType, Exception ex)
        {
            const string indent = "   ";

            Console.WriteLine("Input: {0}", input.Show());
            Console.WriteLine("Pattern: {0}", pattern.ShowVerbatim());

            Console.WriteLine("Exception was expected, this was thrown ({0}):", algorithmType.ToString());

            for (Exception e = ex; e != null; e = e.InnerException)
            {
                Console.WriteLine(indent + "[{0}]", e.GetType().FullName);
                Console.WriteLine(indent + indent + e.Message.Replace("\n", "\n" + indent + indent));
            }

            Console.Write("\n");
        }

        private static Match2 createMatch(Msoft.Match msoftMatch)
        {
            if (msoftMatch.Success)
                return Factory.CreateMatch(msoftMatch.Index, msoftMatch.Length, msoftMatch.Value);
            else
                return Match2.Empty;
        }

        private static string formatException(string message, string input, string pattern, Exception ex)
        {
            const string indent = "  ";

            if (string.IsNullOrEmpty(message))
                message = "";
            else
                message += "\n";

            message += string.Format("Comparing with .NET Regex: Input={0}, Pattern={1}\n", input.Show(), pattern.ShowVerbatim()) +
                       ex.Message;
            message = indent + message.Replace("\n", "\n" + indent);

            return message;
        }

        private static Exception catchException(Action runRegex, string regexName, string input, string pattern, string message)
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
                throw new AssertionException(formatException(message, input, pattern,
                      new AssertionException(regexName + " did not throw an exception, though was supposed to.")));
        }

        private static IEnumerable<Exception> exceptionChain(Exception ex)
        {
            for (Exception e = ex; e != null; e = e.InnerException)
                yield return e;
        }
    }
}
