using System.Collections.Generic;
using System.Linq;

namespace ClickOnce
{
    public class Option
    {
        public Option(ArgsSource source, string name, object value)
        {
            Source = source;
            Name = name;
            Value = value;
        }
        public ArgsSource Source { get; }
        public string Name { get; }
        public object Value { get; }

        public override string ToString()
        {
            var formatted = Value is IEnumerable<string> multi
                ? multi.Any() ? $"[\"{string.Join("\", \"", multi)}\"]" : "[ ]"
                : Value?.ToString();

            return $"{Name}: {formatted ?? "null"}";
        }
    }

    public class Option<T> : Option
    {
        public Option(ArgsSource source, string name, T value)
            : base(source, name, value)
        {
            Value = value;
        }
        public new T Value { get; }
    }

    public class StringOption : Option<string>
    {
        public StringOption(Option<string> option)
            : base(option.Source, option.Name, option.Value) { }

        public StringOption(ArgsSource source, string name, string value)
            : base(source, name, value) { }
    }

    public class PathOption : Option<string>
    {
        public PathOption(Option<string> option, string rootedPath)
            : base(option.Source, option.Name, option.Value)
        {
            RootedPath = rootedPath;
        }

        public string RootedPath { get; }

    }
}
