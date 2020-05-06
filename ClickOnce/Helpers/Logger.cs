using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClickOnce.Resources;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace ClickOnce
{
    internal static class Logger
    {
        private static LogLevel Level { get; set; } = LogLevel.Normal;

        internal static void SetLevel(bool? quiet, bool? verbose)
        {
            if (quiet ?? false)
            {
                Level = LogLevel.Quiet;
            }

            if (verbose ?? false)
            {
                Level = LogLevel.Verbose;
            }
        }

        internal static void Banner()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Messages.Build_Banner);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        internal static void Args(Project args)
        {
            if (Level != LogLevel.Verbose) return;
            
            Verbose(Messages.Build_Logging_Verbose, 0, 2);
            Verbose(Messages.Build_Args, 0, 2);
            Group(Messages.Build_Args_CommandLine, args.Where(arg => arg?.Source == ArgsSource.CommandLine));
            Group(Messages.Build_Args_Settings, args.Where(arg => arg?.Source == ArgsSource.Settings));
            Group(Messages.Build_Args_Inferred, args.Where(arg => arg?.Source == ArgsSource.Inferred));

            void Group(string header, IEnumerable<Option> group)
            {
                if (!group.Any()) return;
                Verbose(header, 1);
                foreach (var arg in group)
                {
                    Verbose(arg, 2);
                }
                Verbose();
            }
        }

        internal static void Verbose(object message = null, byte indent = 0, byte newLines = 1, params string[] args)
        {
            var write = message?.ToString() ?? "";
            if ((int)Level > (int)LogLevel.Verbose) return;
            for (byte i = 0; i < indent; i++)
                Console.Write("  ");
            Console.Write(write, args);
            for (byte i = 0; i < newLines; i++)
                Console.WriteLine();
        }

        internal static void Normal(object message = null, byte indent = 0, byte newLines = 1, params string[] args)
        {
            var write = message?.ToString() ?? "";
            if ((int)Level > (int)LogLevel.Normal) return;
            for (byte i = 0; i < indent; i++)
                Console.Write("  ");
            Console.Write(write, args);
            for (byte i = 0; i < newLines; i++)
                Console.WriteLine();
        }

        internal static void Quiet(object message = null, byte indent = 0, byte newLines = 1, params string[] args)
        {
            var write = message?.ToString() ?? "";
            for (byte i = 0; i < indent; i++)
                Console.Write("  ");
            Console.Write(write, args);
            for (byte i = 0; i < newLines; i++)
                Console.WriteLine();
        }

        internal static void OutputMessages(OutputMessageCollection messages, byte indent = 0)
        {
            foreach (OutputMessage message in messages)
            {
                switch (message.Type)
                {
                    case OutputMessageType.Info:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Verbose($"{message.Type}: {message.Text}", indent);
                        break;

                    case OutputMessageType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Normal($"{message.Type}: {message.Text}", indent);
                        break;

                    case OutputMessageType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Quiet($"{message.Type}: {message.Text}", indent);
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void Fatal(Exception exception)
        {
            var inner = exception;
            while (inner is TargetInvocationException && inner.InnerException != null)
            {
                inner = inner.InnerException;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Messages.Build_Exceptions_Fatal, inner.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
