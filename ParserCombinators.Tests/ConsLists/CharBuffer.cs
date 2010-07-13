using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.Tests.ConsLists
{
    public class CharBuffer
    {
        public CharBuffer(string value)
        {
            buffer = value.ToCharArray();
            index = 0;
        }

        private CharBuffer(char[] buffer, int index)
        {
            this.buffer = buffer;
            this.index = index;
        }

        private readonly char[] buffer;
        private readonly int index;

        public CharBuffer MoveNext()
        {
            return new CharBuffer(buffer, index + 1);
        }

        public bool IsAtEnd
        {
            get { return index >= buffer.Length; }
        }

        public char Current
        {
            get { return buffer[index]; }
        }

        public string FromCurrent
        {
            get { return new string(buffer.Skip(index).ToArray()); }
        }

        public override string ToString()
        {
            return FromCurrent;
        }
    }
}
