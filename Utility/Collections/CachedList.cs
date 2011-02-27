using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility.Collections
{
    public class CachedList<T> : IEnumerable<T>
    {
        public CachedList(IEnumerable<T> originalColl)
        {
            if (originalColl == null)
                throw new ArgumentNullException("originalColl.", "Could not build CachedList.");
            
            originalEnumerator = originalColl.GetEnumerator();

            OriginalIndex = -1;
            IsOriginalAfterEnd = false;
        }

        private List<T> cache = new List<T>();
        
        private IEnumerator<T> originalEnumerator;

        public int OriginalIndex { get; private set; }
        public bool IsOriginalAfterEnd { get; private set; }

        public T this[int index]
        {
            get
            {
                if (IsValidIndex(index))
                    return cache[index];
                else
                    throw new ArgumentOutOfRangeException(
                                    "index",
                                    "Index was out of range. Must be non-negative and less than the size of the collection.");
            }
        }

        public bool IsValidIndex(int index)
        {
            if (index < 0)
                return false;

            while (OriginalIndex < index)
            {
                IsOriginalAfterEnd = IsOriginalAfterEnd || !originalEnumerator.MoveNext();

                if (IsOriginalAfterEnd)
                    return false;

                cache.Add(originalEnumerator.Current);
                OnNextOriginal(originalEnumerator.Current, ++OriginalIndex);
            }

            return true;
        }

        protected virtual void OnNextOriginal(T item, int originalIndex)
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>
        {
            public Enumerator(CachedList<T> parent)
            {
                this.currentIndex = -1;
                this.current = default(T);
                this.parent = parent;
            }

            private int currentIndex;
            private T current;
            private CachedList<T> parent;

            object IEnumerator.Current { get { return current; } }
            T IEnumerator<T>.Current { get { return current; } }

            public bool MoveNext()
            {
                if (parent.IsValidIndex(currentIndex + 1))
                {
                    current = parent[++currentIndex];
                    return true;
                }
                else
                    return false;
            }

            public void Reset()
            {
                currentIndex = -1;
            }

            public void Dispose() { }
        }
    }
}
