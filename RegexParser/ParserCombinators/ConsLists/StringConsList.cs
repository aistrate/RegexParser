using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators.ConsLists
{
    /// <summary>
    /// The simplest of ConsLists, although inefficient.
    /// </summary>
    public class StringConsList : IConsList<char>
    {
        public StringConsList(string value)
        {
            Value = value;
        }

        public StringConsList(char[] array)
            : this(new string(array))
        {
        }

        public StringConsList(IEnumerable<char> collection)
            : this(collection.ToArray())
        {
        }

        public string Value { get; private set; }

        public char Head { get { return Value[0]; } }

        public IConsList<char> Tail { get { return new StringConsList(Value.Substring(1)); } }

        public bool IsEmpty { get { return Value.Length == 0; } }
    }
}
