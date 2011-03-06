using System;

namespace Utility.PrettyPrint
{
    public abstract class FormatSpecifier
    {
        public abstract string NewlineString { get; }
        public abstract string IndentString { get; }

        public abstract FormatSpecifier Recalc(string tag);
    }


    public class StaticFormatSpecifier : FormatSpecifier
    {
        public StaticFormatSpecifier(string newlineString, string indentString)
        {
            this.newlineString = newlineString;
            this.indentString = indentString;
        }

        private string newlineString;
        private string indentString;

        public override string NewlineString { get { return newlineString; } }
        public override string IndentString { get { return indentString; } }

        public override FormatSpecifier Recalc(string tag)
        {
            return this;
        }

        public static readonly StaticFormatSpecifier Collapsed = new StaticFormatSpecifier(" " , ""    );
        public static readonly StaticFormatSpecifier Expanded  = new StaticFormatSpecifier("\n", "    ");
    }


    public class VariableFormatSpecifier : FormatSpecifier
    {
        public VariableFormatSpecifier(StaticFormatSpecifier defaultFormatSpecifier,
                                       Func<string, StaticFormatSpecifier, StaticFormatSpecifier> getFormatSpecifier)
        {
            DefaultFormatSpecifier = defaultFormatSpecifier;

            if (getFormatSpecifier == null)
                throw new ArgumentNullException("getFormatSpecifier.", "VariableFormatSpecifier is missing Tag-to-FormatSpecifier function.");

            GetFormatSpecifier = getFormatSpecifier;
        }

        public StaticFormatSpecifier DefaultFormatSpecifier { get; private set; }
        public Func<string, StaticFormatSpecifier, StaticFormatSpecifier> GetFormatSpecifier { get; private set; }

        public override string NewlineString { get { return DefaultFormatSpecifier.NewlineString; } }
        public override string IndentString { get { return DefaultFormatSpecifier.IndentString; } }

        public override FormatSpecifier Recalc(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return this;

            StaticFormatSpecifier newFormatSpecifier = GetFormatSpecifier(tag, DefaultFormatSpecifier);
            if (newFormatSpecifier == null)
                return this;

            return new VariableFormatSpecifier(newFormatSpecifier, GetFormatSpecifier);
        }

        public static VariableFormatSpecifier ExpandedExceptTags(params string[] collapsedTags)
        {
            return new VariableFormatSpecifier(
                            StaticFormatSpecifier.Expanded,

                            (tag, parentFormatSpecifier) =>
                            {
                                if (string.IsNullOrEmpty(tag))
                                    return null;
                                else if (parentFormatSpecifier == StaticFormatSpecifier.Collapsed ||
                                         Array.IndexOf(collapsedTags, tag) >= 0)
                                    return StaticFormatSpecifier.Collapsed;
                                else
                                    return StaticFormatSpecifier.Expanded;
                            });
        }
    }
}
