
namespace ClickOnce
{
    public class DefaultArgs : Args
    {
        public DefaultArgs()
            : base(ArgsSource.Default)
        {
        }

        public override string Target => "publish";
        public override Platform Platform => new Platform("msil");
        public override string Culture => "neutral";
        public override string TargetFramework => "net472";
    }
}
