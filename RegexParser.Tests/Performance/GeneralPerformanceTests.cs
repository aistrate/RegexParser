using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;
using RegexParser.Util;
using RegexParser.Tests.Helpers;

namespace RegexParser.Tests.Performance
{
    public static class GeneralPerformanceTests
    {
        public static void StringConcat()
        {
            DateTime start = DateTime.Now;

            const int strCount = 1000, charCount = 5000;
            string[] ss = Enumerable.Range(0, strCount)
                                    .Select(i => Enumerable.Repeat(i.ToString().PadLeft(4, ' '), charCount / 4)
                                                           .JoinStrings())
                                    .ToArray();

            Console.WriteLine("Create: {0}", DateTime.Now - start);
            start = DateTime.Now;

            string result = "";
            for (int i = 0; i < strCount; i++)
                result += ss[i];

            Console.WriteLine("Append: {0}", DateTime.Now - start);

            // strCount = 1000, charCount = 1000
            //Create: 00:00:00.0624996
            //Append: 00:00:01.2656169

            // strCount = 1000, charCount = 5000
            //Create: 00:00:00.2656233
            //Append: 00:00:06.4687086

            // strCount = 1000, charCount = 10000
            //Create: 00:00:00.5312466
            //Append: 00:00:12.9217923

            // strCount = 5000, charCount = 1000
            //Create: 00:00:00.2656233
            //Append: 00:00:32.7810402

            // Create: O(n) for strCount and charCount
            // Create: O(n^2) for strCount, O(n) for charCount
        }

        public static void MatchCollections()
        {
            MemoryProfiler memoryProfiler = MemoryProfiler.StartNew();
            int itemCount = 10000000;

            MatchCollection2 matches1 = Factory.CreateMatchCollection(Enumerable.Range(1, itemCount)
                                                                                .Select(i => Factory.CreateMatch(i, i, "x")));
            memoryProfiler.CollectGC();
            Console.WriteLine("Matches: {0:#,##0}", matches1.Count);
            displayMemoryProfiler(memoryProfiler, itemCount);
            // 41   bytes/match (itemCount = 10,000,000)
            // 36.9 bytes/match (itemCount = 13,000,000)


            memoryProfiler.Reset();
            Match2[] matches2 = Enumerable.Range(1, itemCount)
                                          .Select(i => Factory.CreateMatch(i, i, "x"))
                                          .ToArray();
            memoryProfiler.CollectGC();
            Console.WriteLine("Matches: {0:#,##0}", matches2.Length);
            displayMemoryProfiler(memoryProfiler, itemCount);
            // 36 bytes/match (itemCount = 10,000,000)
            // 36 bytes/match (itemCount = 13,000,000)


            memoryProfiler.Reset();
            List<Match2> matches3 = Enumerable.Range(1, itemCount)
                                              .Select(i => Factory.CreateMatch(i, i, "x"))
                                              .ToList();
            memoryProfiler.CollectGC();
            Console.WriteLine("Matches: {0:#,##0}", matches3.Count);
            displayMemoryProfiler(memoryProfiler, itemCount);
            // 38.7 bytes/match (itemCount = 10,000,000)
            // 37.2 bytes/match (itemCount = 13,000,000)


            memoryProfiler.Reset();
            List<int> ints1 = Enumerable.Range(1, itemCount)
                                        .ToList();
            memoryProfiler.CollectGC();
            Console.WriteLine("Ints:    {0:#,##0}", ints1.Count);
            displayMemoryProfiler(memoryProfiler, itemCount);
            // 6.7 bytes/match (itemCount = 10,000,000)
            // 5.2 bytes/match (itemCount = 13,000,000)


            memoryProfiler.Reset();
            int[] ints2 = Enumerable.Range(1, itemCount)
                                    .ToArray();
            memoryProfiler.CollectGC();
            Console.WriteLine("Ints:    {0:#,##0}", ints2.Length);
            displayMemoryProfiler(memoryProfiler, itemCount);
            // 4 bytes/match (itemCount = 10,000,000)
            // 4 bytes/match (itemCount = 13,000,000)
        }

        private static void displayMemoryProfiler(MemoryProfiler memoryProfiler, int itemCount)
        {
            Console.WriteLine("Memory:  {0,11:#,##0} bytes ({1:#,##0.#} per item)\n",
                              memoryProfiler.DeltaValue,
                              (decimal)memoryProfiler.DeltaValue / (decimal)itemCount);
        }
    }
}
