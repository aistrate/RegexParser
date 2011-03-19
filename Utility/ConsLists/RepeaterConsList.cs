using System;

namespace Utility.ConsLists
{
    public class RepeaterConsList<T> : IConsList<T>
    {
        public RepeaterConsList(T value)
            : this(value, null)
        {
        }

        public RepeaterConsList(T value, int? count)
        {
            this.value = value;
            this.count = count;
        }

        private T value;
        private int? count;


        public T Head { get { this.AssertNotEmpty(ConsOp.Head); return value; } }

        public IConsList<T> Tail
        {
            get
            {
                this.AssertNotEmpty(ConsOp.Tail);
                return IsInfinite ? this : new RepeaterConsList<T>(value, (int)count - 1);
            }
        }

        public bool IsEmpty { get { return !IsInfinite && (int)count <= 0; } }

        public int Length
        {
            get
            {
                if (IsInfinite)
                    throw new ApplicationException("Cannot calculate length of infinite list.");

                return (int)count;
            }
        }


        public bool IsInfinite { get { return count == null; } }
    }
}
