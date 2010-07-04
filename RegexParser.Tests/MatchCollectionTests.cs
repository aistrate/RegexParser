using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RegexParser.Tests.Util;

namespace RegexParser.Tests
{
    [TestFixture]
    public class MatchCollectionTests
    {
        [Test]
        public void NullOrEmptyMatch()
        {
            MatchCollection2 coll = matchCollection2_Ctor1(null);
            Assert.AreEqual(0, coll.Count, "Null.");

            coll = matchCollection2_Ctor1(Match2.Empty);
            Assert.AreEqual(0, coll.Count, "Empty/1.");

            coll = matchCollection2_Ctor1(match2_Ctor0());
            Assert.AreEqual(0, coll.Count, "Empty/2.");
        }
        
        [Test]
        public void OneMatch()
        {
            MatchCollection2 coll = matchCollection2_Ctor1(match2_Ctor4(0, 0, "", null));
            Assert.AreEqual(1, coll.Count, "Second null.");

            coll = matchCollection2_Ctor1(match2_Ctor4(0, 0, "", () => Match2.Empty));
            Assert.AreEqual(1, coll.Count, "Second empty.");
        }

        [Test]
        public void TwoMatches()
        {
            MatchCollection2 coll = matchCollection2_Ctor1(match2_Ctor4(100, 10, "One", () =>
                                                         match2_Ctor4(200, 30, "Two", () => Match2.Empty)));
            
            Assert.AreEqual(2, coll.Count, "Second null.");
        }

        [Test]
        public void ManyMatches()
        {
            MatchCollection2 coll = matchCollection2_Ctor1(getMatchChain(1, 0));
            Assert.AreEqual(0, coll.Count, "1 to 0.");

            coll = matchCollection2_Ctor1(getMatchChain(0, 0));
            Assert.AreEqual(1, coll.Count, "From 0 to 0.");

            coll = matchCollection2_Ctor1(getMatchChain(5, 5));
            Assert.AreEqual(1, coll.Count, "From 5 to 5.");

            coll = matchCollection2_Ctor1(getMatchChain(1, 10));
            Assert.AreEqual(10, coll.Count, "From 1 to 10.");

            int from = 1000, to = 4000;
            coll = matchCollection2_Ctor1(getMatchChain(from, to));
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
            MatchCollection2 coll = matchCollection2_Ctor1b(() => getMatchChain(0, counter));
            
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

        private static Match2 getMatchChain(int from, int to)
        {
            if (from <= to)
                return match2_Ctor4(from, from, string.Format("Match{0}", from), () => getMatchChain(from + 1, to));
            else
                return Match2.Empty;
        }

        private static Match2 getMatchChain(int from, Counter counter)
        {
            counter.Inc();
            
            return match2_Ctor4(from, from, string.Format("Match{0}", from), () => getMatchChain(from + 1, counter));
        }

        private static Func<Match2, MatchCollection2> matchCollection2_Ctor1 =
            ClassExtensions.GetConstructor<MatchCollection2, Match2>();

        private static Func<Func<Match2>, MatchCollection2> matchCollection2_Ctor1b =
            ClassExtensions.GetConstructor<MatchCollection2, Func<Match2>>();

        private static Func<Match2> match2_Ctor0 =
            ClassExtensions.GetConstructor<Match2>();

        private static Func<int, int, string, Func<Match2>, Match2> match2_Ctor4 =
            ClassExtensions.GetConstructor<Match2, int, int, string, Func<Match2>>();
    }
}
