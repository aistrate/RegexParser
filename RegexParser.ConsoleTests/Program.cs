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


            MatchCollection matches = Regex.Matches("abc", "x");

            Console.WriteLine();
            Console.WriteLine("matches.Count = {0}", matches.Count);

            matches = Regex.Matches("abc", "b");
            Console.WriteLine("matches[0] = '{0}'", matches[0].Value);


            Console.WriteLine();
            
            {
                List<Match> xx = new List<Match>();
                IEnumerator<Match> e = xx.GetEnumerator();
                e.MoveNext();
                Match x = e.Current;
                e.MoveNext();
                x = e.Current;

                Console.WriteLine("x == null: {0}", x == null);
                //Console.WriteLine("xx[0]: {0}", xx[0]);
            }

            matches = Regex.Matches("abc", "x");
            IEnumerable<Match> xxm = matches.Cast<Match>();
            IEnumerator<Match> em = xxm.GetEnumerator();
            em.MoveNext();
            Match xm = em.Current;
            em.MoveNext();
            xm = em.Current;
            em.MoveNext();
            xm = em.Current;

            Console.WriteLine("xm == null: {0}", xm == null);
            //Console.WriteLine("xm.Success: {0}", xm.Success);
        }
    }
}
