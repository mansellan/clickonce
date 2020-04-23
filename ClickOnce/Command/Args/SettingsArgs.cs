using System;
using Microsoft.Extensions.Configuration;

namespace ClickOnce
{
    internal sealed class SettingsArgs : Args
    {
        public SettingsArgs(string verb)
            : base(ArgsSource.Settings)
        {
            try
            {
                new ConfigurationBuilder()
                    .AddJsonFile("settings.json", true)
                    .Build()
                    .GetSection(verb)
                    .Bind(this);
            }
            catch (Exception exception)
            {
                // TODO - log
            }
        }
    }
}
