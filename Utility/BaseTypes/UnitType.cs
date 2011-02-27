namespace Utility.BaseTypes
{
    /// <summary>
    /// The unit datatype () from Haskell, with only one member, ().
    /// </summary>
    public struct UnitType
    {
        public static UnitType Unit { get { return new UnitType(); } }
    }
}
