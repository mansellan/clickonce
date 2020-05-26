using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ClickOnce.Resources;
using Microsoft.Build.Tasks;
using Microsoft.Build.Utilities;

namespace ClickOnce
{
    internal static class PrerequisitesBuilder
    {
        internal static void Build(Project project)
        {
            if (project.Prerequisites.Value is null || !project.Prerequisites.Value.Any())
                return;

            var location = project.PrerequisitesLocation.Value?.ToLowerInvariant() switch
            {
                "vendor" => "HomeSite",
                "deployment" => "Relative",
                _ => "Absolute"
            };

            var url = location == "Absolute" ? new Uri(project.PrerequisitesLocation.Value, UriKind.Absolute).AbsoluteUri : null;

            var bootstrapper = new GenerateBootstrapper
            {
                ApplicationFile = project.DeploymentManifestFile.Value,
                ApplicationName = project.Product.Value,
                ApplicationUrl = project.DeploymentUrl.Value,
                ComponentsLocation = location,
                ComponentsUrl = url,
                Path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Bootstrapper"),
                OutputPath = project.Target.RootedPath,
                BootstrapperItems = project.Prerequisites.Value.Select(p => new TaskItem(p)).ToArray(),
                Validate = true
            };

            Logger.Normal(Messages.Build_Process_Bootstrapper);
            bootstrapper.Execute();
            Logger.Normal(Messages.Result_Done, 1, 2);
        }
    }
}
