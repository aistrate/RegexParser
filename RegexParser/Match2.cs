using System;
using Utility.BaseTypes;
using Utility.ConsLists;

namespace RegexParser
{
    /// <summary>
    /// Immutable class.
    /// </summary>
    public class Match2 : Group2, IEquatable<Match2>
    {
        internal Match2(int index, int length, string value)
            : this(new Group2(index, length, value))
        {
        }

        internal Match2(Group2 onlyGroup)
        {
            groupConsList = new SimpleConsList<Group2>(onlyGroup);
            firstGroup = onlyGroup;
        }

        internal Match2(Match2 oldMatch, Group2 group)
        {
            groupConsList = new SimpleConsList<Group2>(group, oldMatch.groupConsList);
            firstGroup = oldMatch.firstGroup;
        }

        private SimpleConsList<Group2> groupConsList;
        private Group2 firstGroup;

        public override bool Success { get { return firstGroup.Success; } }
        public override int Index { get { return firstGroup.Index; } }
        public override int Length { get { return firstGroup.Length; } }
        public override string Value { get { return firstGroup.Value; } }

        public static new Match2 Empty = new Match2(Group2.Empty);


        public MatchCollection2 Parent { get; internal set; }
        public int ParentIndex { get; internal set; }

        public Match2 NextMatch()
        {
            return Parent != null && Parent.IsValidIndex(ParentIndex + 1) ? Parent[ParentIndex + 1] : Match2.Empty;
        }


        public override string ToString()
        {
            if (Success)
                return string.Format("Match {{Index={0}, Length={1}, Value={2}}}", Index, Length, Value.Show());
            else
                return string.Format("Match {{Success={0}}}", Success);
        }

        bool IEquatable<Match2>.Equals(Match2 other)
        {
            return other != null &&
                   this.firstGroup.Equals(other.firstGroup);
        }

        public override int GetHashCode()
        {
            return firstGroup.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<Match2>)this).Equals(obj as Match2);
        }
    }
}
