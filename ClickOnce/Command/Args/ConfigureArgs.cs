using ClickOnce.Resources;
using CommandLine;

namespace ClickOnce
{
    [Verb(nameof(CommandVerb.Configure), HelpText = nameof(Messages.Help_Verb_Configure), ResourceType = typeof(Messages))]
    public class ConfigureArgs : Args
    {
        public ConfigureArgs()
            : base(ArgsSource.CommandLine, CommandVerb.Configure)
        {
        }
    }
}