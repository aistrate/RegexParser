using System;
using System.Linq;
using Utility.BaseTypes;
using Utility.ConsLists;
using Utility.General;

namespace RegexParser
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class Group2 : Capture2, IEquatable<Group2>
    {
        protected Group2()
        {
            captureConsList = SimpleConsList<Capture2>.Empty;
        }

        internal Group2(int index, int length, string value)
            : this(new Capture2(index, length, value))
        {
        }

        internal Group2(Capture2 onlyCapture)
        {
            captureConsList = new SimpleConsList<Capture2>(onlyCapture);
        }

        internal Group2(Group2 oldGroup, Capture2 capture)
        {
            captureConsList = new SimpleConsList<Capture2>(capture, oldGroup.captureConsList);
        }

        private SimpleConsList<Capture2> captureConsList;

        public virtual bool Success { get { return !captureConsList.IsEmpty; } }

        public override int Index { get { return Success ? captureConsList.Head.Index : 0; } }
        public override int Length { get { return Success ? captureConsList.Head.Length : 0; } }
        public override string Value { get { return Success ? captureConsList.Head.Value : ""; } }

        public CaptureCollection2 Captures
        {
            get
            {
                if (captures == null)
                    captures = new CaptureCollection2(captureConsList.AsEnumerable().Reverse());

                return captures;
            }
        }
        private CaptureCollection2 captures;

        protected static Group2 Empty = new Group2();


        public override string ToString()
        {
            if (Success)
                return string.Format("Group {{Index={0}, Length={1}, Value={2}}}", Index, Length, Value.Show());
            else
                return string.Format("Group {{Success={0}}}", Success);
        }

        bool IEquatable<Group2>.Equals(Group2 other)
        {
            return other != null &&
                   this.Captures.Count == other.Captures.Count &&
                   this.Captures.SequenceEqual(other.Captures);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Captures.Select(c => c.GetHashCode()).ToArray());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Group2>)this).Equals(obj as Group2);
        }
    }
}
