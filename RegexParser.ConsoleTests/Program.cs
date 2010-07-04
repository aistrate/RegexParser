using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using MSoft = System.Text.RegularExpressions;

namespace RegexParser.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //new RegexTests().CharPatternMatches_Overlap();

                Console.WriteLine(formatMSoftMatches(MSoft.Regex.Matches("This is alfalfa", "alfa")));
                Console.WriteLine(formatMSoftMatches(MSoft.Regex.Matches("This is alfa and alfa", "alfa")));
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

        private static string formatMSoftMatches(MSoft.MatchCollection matches)
        {
            MSoft.Match[] matchArr = matches.Cast<MSoft.Match>().ToArray();

            return string.Format("{0} matches: [", matchArr.Length) +
                   string.Join(", ", matchArr.Select(m => formatMSoftMatch(m)).ToArray()) +
                   "]";
        }

        private static string formatMSoftMatch(MSoft.Match match)
        {
            return string.Format("({0}, {1}, \"{2}\")", match.Index, match.Length, match.Value);
        }
    }
}
