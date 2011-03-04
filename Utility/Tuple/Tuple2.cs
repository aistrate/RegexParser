using System;
using System.Collections.Generic;
using Utility.General;

namespace Utility.Tuple
{
    public class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Item1, Item2);
        }

        bool IEquatable<Tuple<T1, T2>>.Equals(Tuple<T1, T2> other)
        {
            return other != null &&
                   EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) &&
                   EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Item1.GetHashCode(), Item2.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Tuple<T1, T2>>)this).Equals(obj as Tuple<T1, T2>);
        }
    }
}
