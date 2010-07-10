using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.ParserCombinators.ConsLists
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

        public T Head { get { return firstNode.Value; } }

        public IConsList<T> Tail { get { return new LinkedConsList<T>(firstNode.Next); } }

        public bool IsEmpty { get { return firstNode == null; } }
    }
}
