using System.Collections.Generic;

namespace ClickOnce
{
    public class StringsOption : Option<IEnumerable<string>>
    {
        public StringsOption(Option<IEnumerable<string>> option)
            : base(option.Source, option.Name, option.Value) { }
    }
}
