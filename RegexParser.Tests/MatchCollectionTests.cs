using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Tests.Helpers;
using RegexParser.Util;

namespace RegexParser.Tests
{
    [TestFixture]
    public class MatchCollectionTests
    {
        [Test]
        public void Empty()
        {
            MatchCollection2 coll = Factory.CreateMatchCollection(getMatches(0, -1));
            Assert.AreEqual(0, coll.Count, "Empty/IEnumerable.");

            coll = Factory.CreateMatchCollection(new Match2[] { });
            Assert.AreEqual(0, coll.Count, "Empty/Array.");
        }
        
        [Test]
        public void OneMatch()
        {
            MatchCollection2 coll = Factory.CreateMatchCollection(new[] { Factory.CreateMatch(0, 0, "") });
            Assert.AreEqual(1, coll.Count, "Empty.");

            coll = Factory.CreateMatchCollection(new[] { Match2.Empty });
            Assert.AreEqual(1, coll.Count, "Match2.Empty.");
        }

        [Test]
        public void ManyMatches()
        {
            MatchCollection2 coll = Factory.CreateMatchCollection(getMatches(1, 0));
            Assert.AreEqual(0, coll.Count, "1 to 0.");

            coll = Factory.CreateMatchCollection(getMatches(0, 0));
            Assert.AreEqual(1, coll.Count, "From 0 to 0.");

            coll = Factory.CreateMatchCollection(getMatches(5, 5));
            Assert.AreEqual(1, coll.Count, "From 5 to 5.");

            coll = Factory.CreateMatchCollection(getMatches(1, 10));
            Assert.AreEqual(10, coll.Count, "From 1 to 10.");

            int from = 1000, to = 4000;
            coll = Factory.CreateMatchCollection(getMatches(from, to));
            Assert.AreEqual(to - from + 1, coll.Count, string.Format("From {0} to {1}.", from, to));
            
            Assert.AreEqual(to, coll[to - from].Index, "Index");
            Assert.AreEqual(to, coll[to - from].Length, "Length");
            Assert.AreEqual("Match" + to.ToString(), coll[to - from].Value, "Value");
        }

        [Test]
        public void PerformanceTest()
        {
            const int n1 = 100, n2 = 500, n3 = 1800, n4 = 1850, n5 = 3500;
            
            Counter counter = new Counter();
            MatchCollection2 coll = Factory.CreateMatchCollection(getMatches(0, counter));
            
            IEnumerable<Match2> seq1 = coll.Take(n1),
                                seq2 = coll.Take(n2);

            Assert.AreEqual(0, counter.Value, "Start.");

            Assert.AreEqual(n1 - 1, seq1.Last().Index, "First/n1");
            Assert.AreEqual(n1, counter.Value, "First/counter/1");
            Assert.AreEqual(n2 - 1, seq2.Last().Index, "First/n2");
            Assert.AreEqual(n2, counter.Value, "First/counter/2");

            Assert.AreEqual(n1 - 1, seq1.Last().Index, "Second/n1");
            Assert.AreEqual(n2, counter.Value, "Second/counter/1");
            Assert.AreEqual(n2 - 1, seq2.Last().Index, "Second/n2");
            Assert.AreEqual(n2, counter.Value, "Second/counter/2");

            IEnumerable<Match2> seq3 = coll.Take(n3),
                                seq4 = coll.Take(n4),
                                seq5 = coll.Take(n5);

            Assert.AreEqual(n3 - 1, seq3.Last().Index, "First/n3");
            Assert.AreEqual(n3, counter.Value, "First/counter/3");
            Assert.AreEqual(n4 - 1, seq4.Last().Index, "First/n4");
            Assert.AreEqual(n4, counter.Value, "First/counter/4");
            Assert.AreEqual(n5 - 1, seq5.Last().Index, "First/n5");
            Assert.AreEqual(n5, counter.Value, "First/counter/5");
        }

        [Test]
        public static void NextMatchTest()
        {
            const int n1 = 5000, n2 = 5300, n3 = 5500, n4 = 7000;

            Counter counter = new Counter();
            MatchCollection2 coll = Factory.CreateMatchCollection(getMatches(n1, counter));

            Assert.AreEqual(0, counter.Value);

            for (Match2 match = coll.First(); match.Index < n2; match = match.NextMatch()) ;
            Assert.AreEqual(n2 - n1 + 1, counter.Value);

            for (Match2 match = coll.First(); match.Index < n3; match = match.NextMatch()) ;
            Assert.AreEqual(n3 - n1 + 1, counter.Value);

            for (Match2 match = coll.First(); match.Index < n4; match = match.NextMatch()) ;
            Assert.AreEqual(n4 - n1 + 1, counter.Value);

            for (Match2 match = coll.First(); match.Index < n2; match = match.NextMatch()) ;
            Assert.AreEqual(n4 - n1 + 1, counter.Value);
        }

        private static IEnumerable<Match2> getMatches(int from, int to)
        {
            return Enumerable.Range(from, Math.Max(0, to - from + 1))
                             .Select(i => Factory.CreateMatch(i, i, string.Format("Match{0}", i)));
        }

        private static IEnumerable<Match2> getMatches(int from, Counter counter)
        {
            for (int i = from; ; i++)
            {
                counter.Inc();
                yield return Factory.CreateMatch(i, i, string.Format("Match{0}", i));
            }
        }
    }
}
