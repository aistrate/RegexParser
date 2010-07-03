using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser.Util
{
    public class CachedList<T> : IEnumerable<T>
    {
        public CachedList(IEnumerable<T> originalColl)
        {
            if (originalColl == null)
                throw new ArgumentNullException("originalColl", "Could not build CachedList.");
            
            originalEnumerator = originalColl.GetEnumerator();

            LastEvaluatedIndex = -1;
            EvaluatedAll = false;
        }

        private IEnumerator<T> originalEnumerator;

        public int LastEvaluatedIndex { get; private set; }
        public bool EvaluatedAll { get; private set; }


        private List<T> cache = new List<T>();

        private object getItem(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException();

            while (LastEvaluatedIndex < index)
            {
                EvaluatedAll = EvaluatedAll || !originalEnumerator.MoveNext();
                if (EvaluatedAll)
                    return null;

                cache.Add(originalEnumerator.Current);
                LastEvaluatedIndex++;
            }

            return cache[index];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator<T>(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator<T>(this);
        }

        public struct Enumerator<T> : IEnumerator<T>
        {
            public Enumerator(CachedList<T> parent)
            {
                this.currentIndex = -1;
                this.current = null;
                this.parent = parent;
            }

            private int currentIndex;
            private object current;
            private CachedList<T> parent;

            object IEnumerator.Current { get { return current; } }

            T IEnumerator<T>.Current
            {
                get
                {
                    if (current != null)
                        return (T)current;
                    else
                        throw new NullReferenceException("Trying to access object beyond end of collection.");
                }
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                current = parent.getItem(currentIndex + 1);

                if (current != null)
                    currentIndex++;

                return current != null;
            }

            public void Reset()
            {
                currentIndex = -1;
            }
        }
    }
}
