using System.Collections.Generic;
using System.Linq;

namespace Utility.ConsLists
{
    /// <summary>
    /// Traversal is very fast (slightly faster than LinkedConsList).
    /// The constructor is very fast.
    /// </summary>
    public class ArrayConsList<T> : IConsList<T>
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
    }
}
