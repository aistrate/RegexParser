namespace Utility.ConsLists
{
    public interface IConsList<T>
    {
        T Head { get; }

        IConsList<T> Tail { get; }

        bool IsEmpty { get; }
    }
}
