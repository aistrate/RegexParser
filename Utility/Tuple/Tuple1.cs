using System;
using System.Collections.Generic;

namespace Utility.Tuple
{
    public class Tuple<T1> : IEquatable<Tuple<T1>>
    {
        public Tuple(T1 item1)
        {
            Item1 = item1;
        }

        public T1 Item1 { get; private set; }

        public override string ToString()
        {
            return string.Format("({0})", Item1);
        }

        bool IEquatable<Tuple<T1>>.Equals(Tuple<T1> other)
        {
            return other != null &&
                   EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1);
        }

        public override int GetHashCode()
        {
            return Item1.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Tuple<T1>>)this).Equals(obj as Tuple<T1>);
        }
    }
}
