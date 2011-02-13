using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.ConsLists
{
    /// <summary>
    /// Single-linked list (most similar to Lisp and Haskell lists).
    /// Traversal is very fast, but construction from an existing collection is slow.
    /// Front insertion is very fast (with 'Cons'), allowing the list to be constructed piece by piece, in reverse order.
    /// The list is immutable.
    /// </summary>
    public class SimpleConsList<T> : IConsList<T>
    {
        private SimpleConsList()
        {
            this.value = default(T);
            this.next = null;
        }

        public SimpleConsList(T value, SimpleConsList<T> next)
        {
            if (next == null)
                throw new NullReferenceException("Reference to next node in SimpleConsList cannot be null.");

            this.value = value;
            this.next = next;
        }

        private T value;
        private SimpleConsList<T> next;


        public T Head { get { assertNotEmpty("Head"); return value; } }

        public IConsList<T> Tail { get { assertNotEmpty("Tail"); return next; } }

        public bool IsEmpty { get { return next == null; } }


        public static SimpleConsList<T> Empty = new SimpleConsList<T>();

        public static SimpleConsList<T> Create(IEnumerable<T> collection)
        {
            SimpleConsList<T> head = Empty;

            foreach (T val in collection.Reverse())
                head = new SimpleConsList<T>(val, head);

            return head;
        }


        public int Length { get { return IsEmpty ? 0 : (1 + next.Length); } }

        public SimpleConsList<T> Prepend(T val)
        {
            return new SimpleConsList<T>(val, this);
        }


        private void assertNotEmpty(string operation)
        {
            if (IsEmpty)
                throw new ApplicationException(
                    string.Format("{0}: could not perform operation because cons list is empty.", operation));
        }
    }
}
