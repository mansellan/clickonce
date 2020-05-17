using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
                DeploymentUrl = project.DeploymentUrl.Value is null ? null : Path.Combine(project.DeploymentUrl.Value, Path.GetFileName(project.DeploymentManifestFile.Value)),
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

            Logger.Normal(Messages.Build_Process_Deployment);
            deployment.Validate();
            Logger.OutputMessages(deployment.OutputMessages, 1);

            if (!project.PackageMode.Value.HasFlag(PackageMode.Deployment))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(project.DeploymentManifestFile.RootedPath));
            ManifestWriter.WriteManifest(deployment, project.DeploymentManifestFile.RootedPath, project.TargetFramework.Version);
            var signed = Utilities.Sign(project.DeploymentManifestFile.RootedPath, project);
            File.Copy(project.DeploymentManifestFile.RootedPath, Path.Combine(project.PackagePath.RootedPath, Path.GetFileName(project.DeploymentManifestFile.Value)), true);
            Logger.Normal(Messages.Build_Process_Manifest, 1, 1, project.DeploymentManifestFile.RootedPath);
            if (signed)
            {
                Logger.Normal(Messages.Build_Process_Manifest_Signed, 1);
            }
            Logger.Normal();

            if (project.CreateAutoRun.Value)
            {
                Logger.Normal(Messages.Build_Proces_AutoRun);
                using var autoRunFile = new StreamWriter(Path.Combine(project.Target.RootedPath, "autorun.inf"));
                autoRunFile.WriteLine("[autorun]");
                autoRunFile.WriteLine($"open={Path.GetFileName(project.DeploymentManifestFile.Value)}");
                Logger.Normal(Messages.Result_Done, 1, 2);
            }

            if (project.DeploymentPage.Value != null)
            {
                Logger.Normal(Messages.Build_Process_DeploymentPage);

                var templateFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "publish.template.html");
                if (!File.Exists(templateFile))
                {
                    Logger.Normal(Messages.Build_Exceptions_DeploymentPage_TemplateNotFound);
                }
                else
                {
                    using var reader = new StreamReader(templateFile);
                    var deploymentPage = reader.ReadToEnd();
                    deploymentPage = Regex.Replace(deploymentPage, @"\$\{PublishedAt\}", $"{DateTime.UtcNow:G} (UTC)", RegexOptions.IgnoreCase);

                    var placeholder = Regex.Match(deploymentPage, @"\$\{\w+\}", RegexOptions.IgnoreCase);
                    while (placeholder.Success)
                    {
                        var option = project.FirstOrDefault(o => o.Name.Equals(placeholder.Value.Substring(2, placeholder.Length - 3), StringComparison.InvariantCultureIgnoreCase));
                        if (option != null)
                        {
                            deploymentPage = deploymentPage.Replace(placeholder.Value, option.ToString());
                        }

                        placeholder = placeholder.NextMatch();
                    }

                    using var writer = new StreamWriter(Path.Combine(project.Target.RootedPath, project.DeploymentPage.Value));
                    writer.Write(deploymentPage);
                    Logger.Normal(Messages.Result_Done, 1, 2);
                }
            }
        }
    }
}
