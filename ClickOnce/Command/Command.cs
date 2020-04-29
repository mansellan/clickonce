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
                .ParseArguments<CreateArgs>(args)
                .MapResult(
                    (CreateArgs parsed) => ProjectBuilder.Build(parsed),
                    _ => 1);
        }
    }
}
