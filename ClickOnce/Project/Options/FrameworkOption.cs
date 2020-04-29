namespace ClickOnce
{
    public class FrameworkOption : Option<string>
    {
        public FrameworkOption(Option<string> option)
            : base(option.Source, option.Name, option.Value) { }

        public string Moniker => $".NET Framework, Version=v{Version}";

        public string Version => string.Join(".", Value.Substring(3).ToCharArray());
    }
}
