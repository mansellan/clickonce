namespace ClickOnce
{
    public class BooleanOption : Option<bool>
    {
        public BooleanOption(Option<bool> option)
            : base(option.Source, option.Name, option.Value) { }
    }
}
