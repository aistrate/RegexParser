using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTesting
{
    /// <summary>
    /// Class written before "finding" class NUnit.Framework.CollectionAssert.
    /// </summary>
    public static class SeqAssert
    {
        public static void AreEqual<T>(IEnumerable<T> expectedSeq, IEnumerable<T> actualSeq)
        {
            AreEqual(expectedSeq, actualSeq, null);
        }

        public static void AreEqual<T>(IEnumerable<T> expectedSeq, IEnumerable<T> actualSeq, string message)
        {
            try
            {
                if (expectedSeq == null && actualSeq == null)
                    return;

                Assert.IsNotNull(expectedSeq, "Sequence is null.");
                Assert.IsNotNull(actualSeq, "Sequence is null.");

                IEnumerator<T> enumExpected = expectedSeq.GetEnumerator(),
                               enumActual = actualSeq.GetEnumerator();

                bool hasNext_Expected = false, hasNext_Actual = false;
                int index = 0;

                while (true)
                {
                    hasNext_Expected = enumExpected.MoveNext();
                    hasNext_Actual = enumActual.MoveNext();

                    if (!hasNext_Expected || !hasNext_Actual)
                        break;

                    Assert.AreEqual(enumExpected.Current,
                                    enumActual.Current,
                                    string.Format("At index {0}:", index++));
                }

                if (hasNext_Expected)
                    Assert.Fail("Sequence shorter than expected.\nFirst missing element: {0} (index {1}).",
                                enumExpected.Current, index);

                if (hasNext_Actual)
                    Assert.Fail("Sequence longer than expected.\nFirst extra element: {0} (index {1}).",
                                enumActual.Current, index);
            }
            catch (Exception ex)
            {
                const string indent = "  ";

                if (string.IsNullOrEmpty(message))
                    message = "";
                else
                    message += "\n";

                message += "Sequences not equal.\n" + ex.Message;
                message = indent + message.Replace("\n", "\n" + indent);

                throw new AssertionException(message);
            }
        }
    }

    //[TestFixture]
    public class SeqAssertTests
    {
        //[Test]
        public void AreEqualTest()
        {
            //SeqAssert.AreEqual(null, new int[] { 12345 }, "First seq null.");
            //SeqAssert.AreEqual(new int[] { 12345 }, null, "Second seq null.");
            //SeqAssert.AreEqual(new int[] { 12345 }, new int[] { 12346 }, "One elem each.");
            //SeqAssert.AreEqual(new int[] { 12345, 123, 400 }, new int[] { 12345, 124, 400 }, "Second elem.");
            //SeqAssert.AreEqual(new int[] { 12345, 150 }, new int[] { 12345 }, "First seq longer.");
            //SeqAssert.AreEqual(new int[] { 12345, 150 }, new int[] { 12345, 150, 300 }, "Second seq longer.");
        }
    }
}
