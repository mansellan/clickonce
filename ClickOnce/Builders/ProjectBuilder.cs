using System;
using ClickOnce.Resources;
using static ClickOnce.Utilities;

namespace ClickOnce
{
    internal static class ProjectBuilder
    {
        internal static int Build(Args args)
        {
            try
            {
                Logger.Banner();

                var settings = new SettingsArgs(args.Key);
                var inferred = new InferredArgs(args, settings);
                var project = new Project(args, settings, inferred);
                
                Logger.SetLevel(project.Quiet.Value, project.Verbose.Value);
                Logger.Normal(GetMessage($"Build.Verb.{args.Key}"), 0, 2, project.Source.RootedPath);
                Logger.Args(project);

                Logger.Normal(Messages.Build_Process_Project_Validating);
                project.Validate();
                Logger.Normal(Messages.Result_Done, 1, 2);

                ApplicationBuilder.Build(project);
                DeploymentBuilder.Build(project);

                return 0;
            }
            catch (Exception exception)
            {
                Logger.Fatal(exception);
                return 1;
            }
        }
    }
}
