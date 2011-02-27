namespace UnitTesting
{
    /// <summary>
    /// Object that counts the number of times a method has been used.
    /// </summary>
    public class Counter
    {
        public int Value { get; private set; }

        public int Inc() { return ++Value; }

        public void Reset() { Value = 0; }
    }
}
