using System;
using System.Collections.Generic;
using System.Linq;
using Utility.General;

namespace Utility.ConsLists
{
    /// <summary>
    /// Traversal is very fast (slightly faster than LinkedConsList).
    /// The constructor is very fast.
    /// </summary>
    public class ArrayConsList<T> : IConsList<T>, IEquatable<ArrayConsList<T>>
    {
        public ArrayConsList(IEnumerable<T> collection)
            : this(collection.ToArray(), 0)
        {
        }

        public ArrayConsList(T[] array)
            : this(array, 0)
        {
        }

        private ArrayConsList(T[] array, int index)
        {
            this.array = array;
            this.index = index;
        }

        private T[] array;
        private int index;


        public T Head { get { this.AssertNotEmpty(ConsOp.Head); return array[index]; } }

        public IConsList<T> Tail { get { this.AssertNotEmpty(ConsOp.Tail); return new ArrayConsList<T>(array, index + 1); } }

        public bool IsEmpty { get { return index >= array.Length; } }


        public int Length { get { return array.Length - index; } }

        public bool IsStartOfArray { get { return index <= 0; } }

        public T Prev { get { return array[index - 1]; } }


        bool IEquatable<ArrayConsList<T>>.Equals(ArrayConsList<T> other)
        {
            return other != null &&
                   this.index == other.index &&
                   this.array == other.array;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(array.GetHashCode(), index.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<ArrayConsList<T>>)this).Equals(obj as ArrayConsList<T>);
        }
    }
}
