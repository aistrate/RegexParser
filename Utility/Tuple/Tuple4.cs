using System;
using System.Collections.Generic;
using Utility.General;

namespace Utility.Tuple
{
    public class Tuple<T1, T2, T3, T4> : IEquatable<Tuple<T1, T2, T3, T4>>
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public T3 Item3 { get; private set; }
        public T4 Item4 { get; private set; }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", Item1, Item2, Item3, Item4);
        }

        bool IEquatable<Tuple<T1, T2, T3, T4>>.Equals(Tuple<T1, T2, T3, T4> other)
        {
            return other != null &&
                   EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) &&
                   EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) &&
                   EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) &&
                   EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Item1.GetHashCode(), Item2.GetHashCode(), Item3.GetHashCode(), Item4.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Tuple<T1, T2, T3, T4>>)this).Equals(obj as Tuple<T1, T2, T3, T4>);
        }
    }
}
