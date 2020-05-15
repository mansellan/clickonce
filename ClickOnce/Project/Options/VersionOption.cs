using System;
using ClickOnce.Resources;

namespace ClickOnce
{
    public class VersionOption : Option<VersionSettings>
    {
        private static DateTime now = DateTime.UtcNow;

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
                var major = Parse(elements[0]);
                var minor = elements.Length > 1 ? Parse(elements[1]) : "0";
                var build = elements.Length > 2 ? Parse(elements[2]) : "0";
                var revision = elements.Length > 3 ? Parse(elements[3]) : "0";

                string Parse(string segment)
                {
                    return int.TryParse(segment, out _)
                        ? segment
                        : now.ToString(segment);
                }

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
        public VersionSettings(string major, string minor, string build, string revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        private string Major { get; }
        private string Minor { get; }
        private string Build { get; }
        private string Revision { get; }

        public static implicit operator string(VersionSettings settings) => 
            settings is null 
                ? null 
                :  $"{settings.Major}.{settings.Minor}.{settings.Build}.{settings.Revision}";
    }
}
