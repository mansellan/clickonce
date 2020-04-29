namespace ClickOnce
{
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
