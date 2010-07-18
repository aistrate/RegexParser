using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParserCombinators.Util;
using RegexParser.Util;

namespace RegexParser.Tests.Helpers
{
    public class ExceptionWrapper : IEquatable<ExceptionWrapper>
    {
        public ExceptionWrapper(Exception ex)
            : this(ex.GetType(), ex.Message) { }

        public ExceptionWrapper(Type type, string message)
        {
            Type = type;
            Message = message;
        }

        public Type Type { get; private set; }
        public string Message { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {{{1}}}", Type.FullName, Message.Replace("\n", "\\n"));
        }

        public static ExceptionWrapper Create(Exception ex)
        {
            return new ExceptionWrapper(ex);
        }

        bool IEquatable<ExceptionWrapper>.Equals(ExceptionWrapper other)
        {
            return other != null && this.Type == other.Type && this.Message == other.Message;
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Combine(Type.GetHashCode(), Message.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((IEquatable<ExceptionWrapper>)this).Equals(obj as ExceptionWrapper);
        }
    }
}
