using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators.ConsLists
{
    public class RepeaterConsList<T> : IConsList<T>
    {
        public RepeaterConsList(T value)
        {
            this.value = value;
            this.count = null;
        }

        public RepeaterConsList(T value, int count)
        {
            this.value = value;
            this.count = count;
        }

        private T value;
        private int? count;


        public T Head { get { assertNotEmpty("Head"); return value; } }

        public IConsList<T> Tail
        {
            get
            {
                assertNotEmpty("Tail");
                return IsInfinite ? this : new RepeaterConsList<T>(value, (int)count - 1);
            }
        }

        public bool IsEmpty { get { return !IsInfinite && (int)count <= 0; } }


        public bool IsInfinite { get { return count == null; } }

        private void assertNotEmpty(string operation)
        {
            if (IsEmpty)
                throw new ApplicationException(
                    string.Format("{0}: could not perform operation because cons list is empty.", operation));
        }
    }
}
