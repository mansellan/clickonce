using System.Collections.Generic;
using System.Linq;

namespace ClickOnce
{
    public abstract class Option
    {
        protected Option(ArgsSource source, string name, object value)
        {
            Source = source;
            Name = name;
            Value = value;
        }
        public ArgsSource Source { get; }
        public string Name { get; }
        private object Value { get; }

        public override string ToString()
        {
            return Value is IEnumerable<string> enumerable
                ? enumerable.Any() ? string.Join(":", enumerable) : ""
                : Value?.ToString();
        }
    }

    public class Option<T> : Option
    {
        public Option(ArgsSource source, string name, T parsed, object raw = null)
            : base(source, name, raw ?? parsed)
        {
            Value = parsed;
        }
        public T Value { get; }
    }
}
