using System.Security;

namespace ClickOnce
{
    public class SecureOption : Option<SecureString>
    {
        public SecureOption(Option<string> option)
            : base(option.Source, option.Name, Convert(option.Value), "***") { }

        private static SecureString Convert(string clearText)
        {
            if (clearText is null)
                return null;

            var ret = new SecureString();
            foreach (var c in clearText)
            {
                ret.AppendChar(c);
            }

            return ret;
        }
    }
}
