using System;
using System.Collections.Generic;
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
                    })
                //.ParseArguments<CreateArgs, UpdateArgs, BuildArgs, ConfigureArgs>(args)
                .ParseArguments<CreateArgs>(args)
                .MapResult(
                    (CreateArgs parsed) => ProjectBuilder.Create(parsed),
                    //(UpdateArgs parsed) => Update(parsed),
                    //(BuildArgs parsed) => Build(parsed),
                    //(ConfigureArgs parsed) => Configure(parsed),
                    HandleParseErrors);
        }


        //private static int Update(UpdateArgs args)
        //{
        //    Console.WriteLine("Update not yet implemented :-(");
        //    // TODO!!
        //    return 1;
        //}

        //private static int Build(BuildArgs options)
        //{
        //    // TODO!!
        //    return 1;
        //}

        //private static int Configure(ConfigureArgs args)
        //{
        //    return 0;
        //}

        private static int HandleParseErrors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                switch (error.Tag)
                {
                    case ErrorType.SetValueExceptionError:

                        var typed = (SetValueExceptionError)error;
                        Console.WriteLine($"{typed.NameInfo.LongName}: '{typed.Value}' is not valid. {((SetValueExceptionError)error).Exception?.Message}");
                        break;

                    case ErrorType.BadFormatConversionError:

                        var typed1 = (BadFormatConversionError)error;
                        Console.WriteLine($"{typed1.NameInfo.LongName}: ");
                        break;
                }
            }
            // TODO!!
            return 1;
        }
    }
}
