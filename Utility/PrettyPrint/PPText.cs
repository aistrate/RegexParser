namespace Utility.PrettyPrint
{
    public class PPText : PPElement
    {
        public PPText(string value)
            : this("", value) { }

        public PPText(string tag, string value)
            : base(tag)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string Format(int indentLevel, FormatSpecifier formatSpecifier)
        {
            return Value;
        }
    }
}
