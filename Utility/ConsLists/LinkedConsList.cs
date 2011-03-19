using System.Collections.Generic;

namespace Utility.ConsLists
{
    /// <summary>
    /// Traversal is very fast, but the constructor is slow.
    /// </summary>
    public class LinkedConsList<T> : IConsList<T>
    {
        public LinkedConsList(IEnumerable<T> collection)
            : this(new LinkedList<T>(collection).First)
        {
        }

        private LinkedConsList(LinkedListNode<T> firstNode)
        {
            this.firstNode = firstNode;
        }

        private LinkedListNode<T> firstNode;


        public T Head { get { this.AssertNotEmpty(ConsOp.Head); return firstNode.Value; } }

        public IConsList<T> Tail { get { this.AssertNotEmpty(ConsOp.Tail); return new LinkedConsList<T>(firstNode.Next); } }

        public bool IsEmpty { get { return firstNode == null; } }

        public int Length { get { return IsEmpty ? 0 : (1 + Tail.Length); } }
    }
}
