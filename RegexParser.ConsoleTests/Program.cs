using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RegexParser;
using RegexParser.Tests;
using RegexParser.Tests.Patterns;
using RegexParser.Util;

namespace RegexParser.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //originalRegexTest();

                new RegexTests().CharPatternMatch();
            }
            catch (Exception ex)
            {
                Console.WriteLine("MESSAGE:\n");
                Console.WriteLine(ex.Message);

                if (!(ex is AssertionException))
                    Console.WriteLine();
                
                Console.WriteLine("STACK TRACE:\n");
                Console.WriteLine(formatStackTrace(ex.StackTrace));
            }
            
            Console.WriteLine();
        }

        private static string formatStackTrace(string stackTrace)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Program));
            string assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(", "));
            int assemblyNameIndex = assembly.Location.IndexOf(assemblyName);

            if (assemblyNameIndex >= 0)
            {
                string parentFolder = assembly.Location.Substring(0, assemblyNameIndex);
                stackTrace = stackTrace.Replace(parentFolder, "");
            }

            return stackTrace.Replace("\n", "\n\n")
                             .Replace(" in ", "\n   in ")
                             .Replace(".cs:line ", ".cs\n   ln ");
        }

        private static void originalRegexTest()
        {
            Match match = Regex.Match("abc", "x");

            Console.WriteLine("Value = '{0}'", match.Value);
            Console.WriteLine("Success = {0}", match.Success);

            Console.WriteLine("NextMatch().Value = '{0}'", match.NextMatch().Value);
            Console.WriteLine("NextMatch().Success = {0}", match.NextMatch().Success);


            MatchCollection matches = Regex.Matches("abc", "x");

            Console.WriteLine();
            Console.WriteLine("matches.Count = {0}", matches.Count);

            matches = Regex.Matches("abc", "b");

            Console.WriteLine();
            Console.WriteLine("matches.Count = {0}", matches.Count);
            Console.WriteLine("matches[0] = '{0}'", matches[0].Value);
        }
    }
}
