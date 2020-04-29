namespace ClickOnce
{
    public class StringOption : Option<string>
    {
        public StringOption(Option<string> option)
            : base(option.Source, option.Name, option.Value) { }
    }
}
