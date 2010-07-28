using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ParserCombinators.Util;
using RegexParser.Tests.Helpers;
using RegexParser.Util;

namespace RegexParser.Tests.Util
{
    [TestFixture]
    public class CachedListTests
    {
        #region Correctness Tests

        [Test]
        public void NullArgument()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                IEnumerable<string> v = new CachedList<string>(null);
            });
        }

        [Test]
        public void ConvertEmpty()
        {
            conversionTest(new string[] { }, "String[]");
            conversionTest(new int[] { }, "Int32[]");
            conversionTest(new Match2[] { }, "Match2[]");
        }

        [Test]
        public void ConvertNullElement()
        {
            conversionTest(new string[] { "" }, "Empty string");
            conversionTest(new string[] { null }, "Null string");
            conversionTest(new int[] { 0 }, "Zero");
            conversionTest(new int?[] { null }, "Null Int32");
            conversionTest(new Regex2[] { null }, "Null Regex2");
        }

        [Test]
        public void ConvertOneElement()
        {
            conversionTest(new string[] { "Something" }, "String[]");
            conversionTest(new int[] { 12345 }, "Int32[]");
            conversionTest(new Regex2[] { new Regex2("abcd") }, "Regex2[]");
        }

        [Test]
        public void ConvertTwoElements()
        {
            conversionTest(new string[] { "James", "Bond" }, "String[]");
            conversionTest(new int[] { 55, 66 }, "Int32[]");
            conversionTest(new Regex2[] { new Regex2("abcd"), new Regex2("(a|b)") }, "Regex2[]");
        }

        [Test]
        public void ConvertManyElements()
        {
            conversionTest(threeLetterStrings, "String[]");
            conversionTest(Enumerable.Range(1000, 2700), "Int32[]");
            conversionTest(threeLetterStrings.Select(s => new Regex2(s)).ToArray(), "Regex2[]");
        }

        private void conversionTest<T>(IEnumerable<T> original, string message)
        {
            CollAssert.AreEqual(original.ToArray(), (new CachedList<T>(original)).ToArray(), message);
        }

        [Test]
        public void MoveAfterEnd_Int32()
        {
            IEnumerable<int> list =
                //new List<int>(new int[] { });
                new CachedList<int>(new int[] { });

            IEnumerator<int> e = list.GetEnumerator();

            Assert.AreEqual(0, e.Current, "Before MoveNext.");
            e.MoveNext();
            Assert.AreEqual(0, e.Current, "After one MoveNext.");
            e.MoveNext();
            Assert.AreEqual(0, e.Current, "After two MoveNext's.");
        }

        [Test]
        public void MoveAfterEnd_String()
        {
            IEnumerable<string> list =
                //new List<string>(new string[] { });
                new CachedList<string>(new string[] { });

            IEnumerator<string> e = list.GetEnumerator();

            Assert.AreEqual(null, e.Current, "Before MoveNext.");
            e.MoveNext();
            Assert.AreEqual(null, e.Current, "After one MoveNext.");
            e.MoveNext();
            Assert.AreEqual(null, e.Current, "After two MoveNext's.");
        }

        [Test]
        public void Indexer()
        {
            CachedList<string> cachedList = new CachedList<string>(threeLetterStrings);

            for (int i = 0; i < threeLetterStrings.Length; i++)
                Assert.AreEqual(threeLetterStrings[i], cachedList[i], "First/at index: " + i.ToString());

            // move past the end
            IEnumerator<string> cachedListEnum = cachedList.GetEnumerator();
            for (int i = 0; i < threeLetterStrings.Length + 1; i++)
                cachedListEnum.MoveNext();

            for (int i = 0; i < threeLetterStrings.Length; i++)
                Assert.AreEqual(threeLetterStrings[i], cachedList[i], "Second/at index: " + i.ToString());
        }

        [Test]
        public void IndexerOutOfRange()
        {
            CachedList<string> cachedList = new CachedList<string>(new string[] { });
            string s;

            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[-1]; });
            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[-7]; });
            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[0]; });
            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[1]; });
            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[10]; });

            cachedList = new CachedList<string>(new string[] { "abc" });

            s = cachedList[0];
            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[1]; });
            Assert.Catch<ArgumentOutOfRangeException>(() => { s = cachedList[-1]; });
        }

        private string[] threeLetterStrings = Enumerable.Range(0, 26)
                                                        .Select(i => (char)(i + (byte)'a'))
                                                        .Select(c => Enumerable.Repeat(c, 3).AsString())
                                                        .ToArray();

        #endregion


        #region Performance Tests

        [Test]
        public void PerformanceTest_IEnumerable()
        {
            Counter counter = new Counter();
            IEnumerable<int> numbersEnum = getNaturalNumbers(counter);

            Assert.AreEqual(0, counter.Value, "First/counter/0");
            
            IEnumerable<int> seq1 = numbersEnum.Take(n1),
                             seq2 = numbersEnum.Take(n2);

            Assert.AreEqual(n1 - 1, seq1.Last(), "First/n1");
            Assert.AreEqual(n1, counter.Value, "First/counter/1");
            Assert.AreEqual(n2 - 1, seq2.Last(), "First/n2");
            Assert.AreEqual(n1 + n2, counter.Value, "First/counter/2");

            Assert.AreEqual(n1 - 1, seq1.Last(), "Second/n1");
            Assert.AreEqual(2 * n1 + n2, counter.Value, "Second/counter/1");
            Assert.AreEqual(n2 - 1, seq2.Last(), "Second/n2");
            Assert.AreEqual(2 * (n1 + n2), counter.Value, "Second/counter/2");
        }

        [Test]
        public void PerformanceTest_List()
        {
            Counter counter = new Counter();
            List<int> numbersList = getNaturalNumbers(counter).Take(n2).ToList();

            Assert.AreEqual(n2, counter.Value, "First/counter/0");

            IEnumerable<int> seq1 = numbersList.Take(n1),
                             seq2 = numbersList.Take(n2);

            Assert.AreEqual(n1 - 1, seq1.Last(), "First/n1");
            Assert.AreEqual(n2, counter.Value, "First/counter/1");
            Assert.AreEqual(n2 - 1, seq2.Last(), "First/n2");
            Assert.AreEqual(n2, counter.Value, "First/counter/2");

            Assert.AreEqual(n1 - 1, seq1.Last(), "Second/n1");
            Assert.AreEqual(n2, counter.Value, "Second/counter/1");
            Assert.AreEqual(n2 - 1, seq2.Last(), "Second/n2");
            Assert.AreEqual(n2, counter.Value, "Second/counter/2");
        }
        
        /// <summary>
        /// We're using the Counter's value as a proxy for performance.
        /// The Counter is incremented every time an element is re-generated by getNaturalNumbers().
        /// </summary>
        [Test]
        public void PerformanceTest_CachedList()
        {
            Counter counter = new Counter();
            CachedList<int> numbersCachedList = new CachedList<int>(getNaturalNumbers(counter));

            Assert.AreEqual(0, counter.Value, "First/counter/0");

            IEnumerable<int> seq1 = numbersCachedList.Take(n1),
                             seq2 = numbersCachedList.Take(n2);

            Assert.AreEqual(n1 - 1, seq1.LastOrDefault(), "First/n1");
            Assert.AreEqual(n1, counter.Value, "First/counter/1");
            Assert.AreEqual(n2 - 1, seq2.LastOrDefault(), "First/n2");
            Assert.AreEqual(n2, counter.Value, "First/counter/2");

            Assert.AreEqual(n1 - 1, seq1.LastOrDefault(), "Second/n1");
            Assert.AreEqual(n2, counter.Value, "Second/counter/1");
            Assert.AreEqual(n2 - 1, seq2.LastOrDefault(), "Second/n2");
            Assert.AreEqual(n2, counter.Value, "Second/counter/2");

            IEnumerable<int> seq3 = numbersCachedList.Take(n3),
                             seq4 = numbersCachedList.Take(n4),
                             seq5 = numbersCachedList.Take(n5);

            Assert.AreEqual(n3 - 1, seq3.LastOrDefault(), "First/n3");
            Assert.AreEqual(n3, counter.Value, "First/counter/3");
            Assert.AreEqual(n4 - 1, seq4.LastOrDefault(), "First/n4");
            Assert.AreEqual(n4, counter.Value, "First/counter/4");
            Assert.AreEqual(n5 - 1, seq5.LastOrDefault(), "First/n5");
            Assert.AreEqual(n5, counter.Value, "First/counter/5");
        }

        private const int n1 = 5000, n2 = 12000,
                          n3 = 15000, n4 = 25000, n5 = 34000;

        private IEnumerable<int> getNaturalNumbers(Counter counter)
        {
            for (int i = 0; ; i++)
            {
                counter.Inc();
                yield return i;
            }
        }

        #endregion
    }
}
