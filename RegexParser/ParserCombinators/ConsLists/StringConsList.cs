using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators.ConsLists
{
    /// <summary>
    /// The simplest of ConsLists, although inefficient.
    /// </summary>
    public class StringConsList : IConsList<Char>
    {
        public StringConsList(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public char Head { get { return Value[0]; } }

        public IConsList<char> Tail { get { return new StringConsList(Value.Substring(1)); } }

        public bool IsEmpty { get { return Value.Length == 0; } }
    }
}
