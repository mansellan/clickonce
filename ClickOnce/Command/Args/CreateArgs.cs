using ClickOnce.Resources;
using CommandLine;

namespace ClickOnce
{
    [Verb(nameof(CommandVerb.Create), HelpText = nameof(Messages.Help_Verb_Create), ResourceType = typeof(Messages))]
    public class CreateArgs : Args
    {
        public CreateArgs()
            : base(ArgsSource.CommandLine, CommandVerb.Create)
        {
        }
    }
}
