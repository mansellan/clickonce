using System;

namespace ClickOnce
{
    public class EnumOption<T> : Option<T>
        where T : struct, Enum
    {
        public EnumOption(Option<string> option, Func<string, T> converter = null)
            : base(option.Source, option.Name, converter?.Invoke(option.Value) ?? Convert(option.Value), option.Value)
        {
        }

        private static T Convert(string value) =>
            (T)Enum.Parse(typeof(T), value, true);
    }
}
