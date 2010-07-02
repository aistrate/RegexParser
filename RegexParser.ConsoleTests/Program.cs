using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RegexParser;

namespace RegexParser.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Match match = Regex.Match("abc", "x");
            
            Console.WriteLine("Value = '{0}'", match.Value);
            Console.WriteLine("Success = {0}", match.Success);
            
            Console.WriteLine("NextMatch().Value = '{0}'", match.NextMatch().Value);
            Console.WriteLine("NextMatch().Success = {0}", match.NextMatch().Success);
        }
    }
}
