namespace RegexParser.Patterns
{
    public abstract class CharPattern : BasePattern
    {
        public CharPattern()
            : base(PatternType.Char, 1)
        {
        }

        public abstract bool IsMatch(char c);
    }
}
