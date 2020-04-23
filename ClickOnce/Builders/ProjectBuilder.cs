using System;
using System.IO;
using ClickOnce.Resources;

// TODO - make this static
namespace ClickOnce
{
    internal class ProjectBuilder
    {
        private readonly Args commandLineArgs;

        private ProjectBuilder(Args args)
        {
            commandLineArgs = args;
        }

        internal Project Args { get; private set; }

        internal static int Create(CreateArgs args)
        { 
            return new ProjectBuilder(args).Build(Verb.Create);
        }

        private int Build(Verb verb)
        {
            try
            {
                Logger.Banner();

                var settings = new SettingsArgs(commandLineArgs.Key);
                var defaults = new DefaultArgs();
                var inferred = new InferredArgs(commandLineArgs, settings, defaults);

                Args = new Project(commandLineArgs, settings, inferred, defaults);
                
                Logger.SetLevel(Args.Quiet.Value, Args.Verbose.Value);
                Logger.Normal(Messages.ResourceManager.GetString($"Build.Verb.{verb}"), 0, 2, Args.Source.RootedPath);
                Logger.Args(Args);

                Validate();

                ApplicationBuilder.Build(this);
                DeploymentBuilder.Build(this);

                return 0;
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception);
                return 1;
            }
        }

        private void Validate()
        {
            if (!Directory.Exists(Args.Source.Value))
            {
                throw new ApplicationException($"'{Args.Source}' does not exist.");
            }

            if (File.Exists(Args.Target.RootedPath))
            {
                throw new ApplicationException($"'{Args.Target}' is a file.");
            }

            if (Args.EntryPoint is null)
            {
                throw new ApplicationException("Entry point not specified, and could not be inferred.");
            }

            if (!File.Exists(Args.EntryPoint.RootedPath))
            {
                throw new ApplicationException($"Entry point assembly '{Args.EntryPoint}' not found.");
            }

            if (!(Args.IconFile.Value is null) && !File.Exists(Args.IconFile.RootedPath))
            {
                throw new ApplicationException($"Icon file '{Args.IconFile}' not found.");
            }
            

            //if (UpdateEnabled && DeploymentUrl is null)
            //{
            //    throw new ApplicationException("DeploymentUrl is required if update mode is not 'off'.");
            //}
        }
    }
}
