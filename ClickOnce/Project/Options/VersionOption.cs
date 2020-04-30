using System;
using ClickOnce.Resources;

namespace ClickOnce
{
    public class VersionOption : Option<VersionSettings>
    {
        public VersionOption(Option<string> option)
            : base(option.Source, option.Name, Convert(option.Value), option.Value)
        {
        }

        private static VersionSettings Convert(string value)
        {
            if (value is null)
                return null;

            try
            {
                var elements = value.Split('.');
                var major = int.Parse(elements[0]);
                var minor = int.Parse(elements[1]);
                var build = elements.Length > 2 ? int.Parse(elements[2]) : 0;
                var revision = elements.Length > 3 ? int.Parse(elements[3]) : 0;

                return new VersionSettings(major, minor, build, revision);
            }
            catch (Exception)
            {
                Logger.Quiet(Messages.Build_Exceptions_VersionNumber_NotValid, 1, 1, value);
            }

            return null;
        }
    }

    public class VersionSettings
    {
        public VersionSettings(int major, int minor, int build, int revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        public int Major { get; }
        public int Minor { get; }
        public int Build { get; }
        public int Revision { get; }

        public static implicit  operator string(VersionSettings settings) => 
            settings is null 
                ? null 
                :  $"{settings.Major}.{settings.Minor}.{settings.Build}.{settings.Revision}";
    }
}
