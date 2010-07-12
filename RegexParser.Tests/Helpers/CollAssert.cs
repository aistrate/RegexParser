using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RegexParser.Tests.Helpers
{
    /// <summary>
    /// Generic equivalent of class NUnit.Framework.CollectionAssert.
    /// </summary>
    public class CollAssert
    {
        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            CollectionAssert.AreEqual(expected, actual);
        }

        public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message)
        {
            CollectionAssert.AreEqual(expected, actual, message);
        }
    }
}
