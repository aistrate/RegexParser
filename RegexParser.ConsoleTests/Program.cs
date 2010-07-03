using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RegexParser;
using RegexParser.Util;

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
                List<int> xxi = new List<int>();
                //Console.WriteLine("xxi[0]: {0}", xxi[0]);
                
                IEnumerator<int> ei = xxi.GetEnumerator();
                ei.MoveNext();
                int xi = ei.Current;
                ei.MoveNext();
                xi = ei.Current;

                Console.WriteLine("xi == null: {0}", xi == null);
                Console.WriteLine("xi: {0}", xi);
            }
            
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

            //IEnumerable<int> c = null;
            //foreach (int xc in c)
            //    Console.WriteLine("xc: {0}", xc);

            //Console.WriteLine(typeof(Array).FullName);

            //IEnumerable<int> numbers = getNaturalNumbers();
            //Console.WriteLine("Count: {0}", numbers.Count());

            //CachedList<string> cachedList = new CachedList<string>(threeLetterStrings);

            //for (int i = 0; i < threeLetterStrings.Length; i++)
            //    Console.WriteLine("First: c[{0}] = {1}", i, cachedList[i]);

            //IEnumerator<string> cachedListEnum = cachedList.GetEnumerator();

            //for (int i = 0; i < 2 * threeLetterStrings.Length; i++)
            //    cachedListEnum.MoveNext();

            //Console.WriteLine();
            //for (int i = 0; i < threeLetterStrings.Length; i++)
            //    Console.WriteLine("Second: c[{0}] = {1}", i, cachedList[i]);


            //MatchCollection2 coll = createMatchCollection2(Match2.Empty);
        }

        //private static MatchCollection2 createMatchCollection2(Match2 firstMatch)
        //{
        //    ConstructorInfo constructorInfo = typeof(MatchCollection2).GetConstructor(new Type[] { typeof(Match2) });

        //    return constructorInfo.Invoke(new object[] { firstMatch }) as MatchCollection2;
        //}

        private static IEnumerable<int> getNaturalNumbers()
        {
            for (int i = 0; ; i++)
                yield return i;
        }
        
        private static string[] threeLetterStrings = Enumerable.Range(0, 26)
                                                               .Select(i => (char)(i + (byte)'a'))
                                                               .Select(c => new string(Enumerable.Repeat(c, 3).ToArray()))
                                                               .ToArray();
    }
}
