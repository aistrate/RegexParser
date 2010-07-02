using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexParser
{
    public class MatchCollection2 : IEnumerable<Match2>
    {
        internal MatchCollection2(Match2 firstMatch)
        {
            //if (firstMatch != null && firstMatch.Success)
            //    matches.Add(firstMatch);

            foundAll = firstMatch == null || !firstMatch.Success;
        }

        private List<Match2> matches = new List<Match2>();
        
        private int lastFoundIndex = -1;
        private bool foundAll = false;

        public Match2 this[int index]
        {
            get
            {
                Match2 match = getItem(index);

                if (match != null)
                    return match;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Match2 getItem(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException();

            while (lastFoundIndex < index)
            {
                //foundAll = foundAll || !OriginalEnumerator.MoveNext();
                //if (foundAll)
                //    return null;

                //matches.Add(OriginalEnumerator.Current);
                //lastFoundIndex++;
            }

            return matches[index];
        }

        public int Count { get { return this.Count(); } }

        public IEnumerator<Match2> GetEnumerator()
        {
            return new Enumerator(matches);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(matches);
        }

        public class Enumerator : IEnumerator<Match2>
        {
            internal Enumerator(IEnumerable<Match2> matches)
            {
            }

            object IEnumerator.Current { get { return null; } }
            public Match2 Current { get { return null; } }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
            }

            public void Dispose()
            {
            }
        }
    }
}
