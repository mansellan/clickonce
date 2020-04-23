using System;
using System.IO;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

// ReSharper disable AssignNullToNotNullAttribute
namespace ClickOnce
{
    internal static class DeploymentBuilder
    {
        internal static void Build(ProjectBuilder projectBuilder)
        {
            try
            {
                var deployment = new DeployManifest
                {
                    SourcePath = projectBuilder.Args.Target.Value,
                    ReadOnly = false,
                    AssemblyIdentity = new AssemblyIdentity
                    {
                        Name = Path.GetFileName(projectBuilder.Args.DeploymentManifestFile.Value),
                        Version = projectBuilder.Args.Version.Value,
                        Culture = projectBuilder.Args.Culture.Value,
                        ProcessorArchitecture = projectBuilder.Args.ProcessorArchitecture.Value
                    },
                    EntryPoint = new AssemblyReference
                    {
                        SourcePath = projectBuilder.Args.ApplicationManifestFile.Value,
                        TargetPath = projectBuilder.Args.ApplicationManifestFile.Value,
                        AssemblyIdentity = AssemblyIdentity.FromManifest(projectBuilder.Args.ApplicationManifestFile.RootedPath)
                    },
                    Product = projectBuilder.Args.Name.Value,
                    SuiteName = projectBuilder.Args.Suite.Value,
                    Publisher = projectBuilder.Args.Publisher.Value,
                    Description = projectBuilder.Args.Description.Value,
                    TargetFrameworkMoniker = projectBuilder.Args.TargetFrameworkMoniker.Value,
                    //DeploymentUrl = project.DeploymentUrl,
                    //ErrorReportUrl = project.ErrorUrl,
                    //SupportUrl = project.SupportUrl,
                    //Install = project.Install,
                    //DisallowUrlActivation = project.DisallowUrlActivation,
                    TrustUrlParameters = projectBuilder.Args.TrustUrlParameters.Value,
                    MapFileExtensions = projectBuilder.Args.UseDeployExtension.Value,
                    CreateDesktopShortcut = projectBuilder.Args.CreateDesktopShortcut.Value,
                    //UpdateEnabled = project.UpdateEnabled,
                    //UpdateInterval = project.UpdateInterval,
                    //UpdateUnit = Utilities.Parse<UpdateUnit>(project.UpdateUnit),
                    //UpdateMode = Utilities.Parse<UpdateMode>(project.UpdateMode)
                };

                deployment.AssemblyReferences.Add(deployment.EntryPoint);
                deployment.ResolveFiles();
                deployment.UpdateFileInfo();
                deployment.Validate();

                Directory.CreateDirectory(Path.GetDirectoryName(projectBuilder.Args.DeploymentManifestFile.RootedPath));
                ManifestWriter.WriteManifest(deployment, projectBuilder.Args.DeploymentManifestFile.RootedPath);
                File.Copy(projectBuilder.Args.DeploymentManifestFile.RootedPath, 
                    Path.Combine(projectBuilder.Args.Target.RootedPath, Path.GetFileName(projectBuilder.Args.DeploymentManifestFile.RootedPath)), true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
