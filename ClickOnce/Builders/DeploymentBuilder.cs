using System.IO;
using ClickOnce.Resources;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

// ReSharper disable AssignNullToNotNullAttribute
namespace ClickOnce
{
    internal static class DeploymentBuilder
    {
        internal static void Build(Project project)
        {
            var deployment = new DeployManifest
            {
                SourcePath = project.Target.Value,
                ReadOnly = false,
                AssemblyIdentity = new AssemblyIdentity
                {
                    Name = Path.GetFileName(project.DeploymentManifestFile.Value),
                    Version = project.Version.Value,
                    Culture = project.Culture.Value,
                    ProcessorArchitecture = project.ProcessorArchitecture.Value.ToString().ToLowerInvariant()
                },
                EntryPoint = new AssemblyReference
                {
                    SourcePath = project.ApplicationManifestFile.RootedPath,
                    TargetPath = Path.Combine(project.PackagePath.Value, project.ApplicationManifestFile.Value),
                    AssemblyIdentity = AssemblyIdentity.FromManifest(project.ApplicationManifestFile.RootedPath)
                },
                Product = project.Product.Value,
                SuiteName = project.Suite.Value,
                Publisher = project.Publisher.Value,
                Description = project.Description.Value,
                TargetFrameworkMoniker = project.TargetFramework.Moniker,
                DeploymentUrl = project.UpdateUrl.Value,
                ErrorReportUrl = project.ErrorUrl.Value,
                SupportUrl = project.SupportUrl.Value,
                Install = project.LaunchMode.Value.HasFlag(LaunchMode.Start),
                DisallowUrlActivation = !project.LaunchMode.Value.HasFlag(LaunchMode.Url),
                TrustUrlParameters = project.TrustUrlParameters.Value,
                MapFileExtensions = project.UseDeployExtension.Value,
                CreateDesktopShortcut = project.CreateDesktopShortcut.Value, 
                UpdateEnabled = project.Update.Value.Enabled,
                UpdateInterval = project.Update.Value.Interval,
                UpdateUnit = project.Update.Value.Unit,
                UpdateMode = project.Update.Value.Mode,
                MinimumRequiredVersion = project.MinimumVersion.Value
            };

            deployment.AssemblyReferences.Add(deployment.EntryPoint);
            deployment.ResolveFiles();
            deployment.UpdateFileInfo(project.TargetFramework.Version);

            Logger.Quiet(Messages.Build_Process_Deployment);
            deployment.Validate();
            Logger.OutputMessages(deployment.OutputMessages, 1);

            if (!project.PackageMode.Value.HasFlag(PackageMode.Deployment))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(project.DeploymentManifestFile.RootedPath));
            ManifestWriter.WriteManifest(deployment, project.DeploymentManifestFile.RootedPath, project.TargetFramework.Version);
            File.Copy(project.DeploymentManifestFile.RootedPath, Path.Combine(project.PackagePath.RootedPath, Path.GetFileName(project.DeploymentManifestFile.Value)), true);
            Logger.Quiet(Messages.Build_Process_Manifest, 1, 2, project.DeploymentManifestFile.RootedPath);
        }
    }
}
