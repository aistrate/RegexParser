using System;
using Utility.BaseTypes;
using Utility.General;

namespace RegexParser
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class Capture2 : IEquatable<Capture2>
    {
        protected Capture2() { }

        internal Capture2(int index, int length, string value)
        {
            this.index = index;
            this.length = length;
            this.value = value;
        }

        private int index;
        private int length;
        private string value;

        public virtual int Index { get { return index; } }
        public virtual int Length { get { return length; } }
        public virtual string Value { get { return value; } }


        public override string ToString()
        {
            return string.Format("Capture {{Index={0}, Length={1}, Value={2}}}", Index, Length, Value.Show());
        }

        bool IEquatable<Capture2>.Equals(Capture2 other)
        {
            return other != null &&
                   this.Index == other.Index &&
                   this.Length == other.Length &&
                   this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Index.GetHashCode(), Length.GetHashCode(), Value.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Capture2>)this).Equals(obj as Capture2);
        }
    }
}
