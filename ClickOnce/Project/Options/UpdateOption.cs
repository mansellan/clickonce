using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace ClickOnce
{
    public class UpdateOption : Option<UpdateSettings>
    {
        public UpdateOption(Option<string> option)
            : base(option.Source, option.Name, Convert(option.Value), option.Value)
        {
        }

        private static UpdateSettings Convert(string value)
        {
            value = (value ?? "none").ToLowerInvariant();
            
            // Value will be one of none, started, starting, or {int}{h|d|w}

            var enabled = value != "none";
            var interval = int.TryParse(value.Substring(0, value.Length - 1), out var parsed) ? parsed : 0;
            var unit = interval == 0
                ? UpdateUnit.Days
                : value.Substring(value.Length - 1) switch
                {
                    "h" => UpdateUnit.Hours,
                    "d" => UpdateUnit.Days,
                    "w" => UpdateUnit.Weeks
                };
            var mode = value == "started" || interval > 0 ? UpdateMode.Background : UpdateMode.Foreground;

            return new UpdateSettings(enabled, interval, unit, mode);
        }
    }

    public class UpdateSettings
    {
        public UpdateSettings(bool enabled, int interval, UpdateUnit unit, UpdateMode mode)
        {
            Enabled = enabled;
            Interval = interval;
            Unit = unit;
            Mode = mode;
        }

        public bool Enabled { get; }
        public int Interval { get; }
        public UpdateUnit Unit { get; }
        public UpdateMode Mode { get; }
    }
}
