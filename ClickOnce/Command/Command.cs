using System;
using CommandLine;

namespace ClickOnce
{
    public static class Command
    {
        public static int Main(string[] args)
        {
            return new Parser(config =>
                    {
                        config.CaseSensitive = false;
                        config.CaseInsensitiveEnumValues = true;
                        config.HelpWriter = Console.Out;
                    })
                .ParseArguments<CreateArgs, ConfigureArgs>(args)
                .MapResult(
                    (CreateArgs parsed) => ProjectBuilder.Build(parsed),
                    (ConfigureArgs parsed) => 
                    {
                        Logger.Quiet("Not yet implemented");
                        return 1;
                    },
                    _ => 1);
        }
    }
}
