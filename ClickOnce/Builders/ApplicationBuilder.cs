using System;
using System.IO;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

// ReSharper disable AssignNullToNotNullAttribute
namespace ClickOnce
{
    internal static class ApplicationBuilder
    {
        internal static void Build(ProjectBuilder projectBuilder)
        {
            var application = new ApplicationManifest
            {
                IsClickOnceManifest = true,
                ReadOnly = false,
                Product = projectBuilder.Args.UseApplicationTrust.Value ? projectBuilder.Args.Name.Value : null,
                Publisher = projectBuilder.Args.UseApplicationTrust.Value ? projectBuilder.Args.Publisher.Value : null,
                Description = projectBuilder.Args.Description.Value,
                SuiteName = projectBuilder.Args.Suite.Value,
                AssemblyIdentity = new AssemblyIdentity
                {
                    Name = Path.GetFileName(projectBuilder.Args.EntryPoint.Value),
                    Version = projectBuilder.Args.Version.Value,
                    Culture = projectBuilder.Args.Culture.Value,
                    ProcessorArchitecture = projectBuilder.Args.ProcessorArchitecture.Value
                },
                EntryPoint = projectBuilder.GetEntryPoint(),
                UseApplicationTrust = projectBuilder.Args.UseApplicationTrust.Value,
                TrustInfo = new TrustInfo() // TODO!!
            };

            application.ResolveGlobs(projectBuilder);
            application.ResolveFiles();
            application.UpdateFileInfo();
            application.Validate();

            Directory.CreateDirectory(Path.GetDirectoryName(projectBuilder.Args.ApplicationManifestFile.RootedPath));
            ManifestWriter.WriteManifest(application, projectBuilder.Args.ApplicationManifestFile.RootedPath);
        }

        private static AssemblyReference GetEntryPoint(this ProjectBuilder projectBuilder)
        {
            var assembly = projectBuilder.Args.EntryPoint.RootedPath;

            var entryPoint = new AssemblyReference(assembly) {AssemblyIdentity = AssemblyIdentity.FromManagedAssembly(assembly) };

            if (entryPoint.AssemblyIdentity is null)
            {
                // TODO - don't throw, make a bootstrapper :-)
                throw new ApplicationException($"Entry point assembly {assembly} is not a managed assembly.");
            }

            entryPoint.AssemblyIdentity.Name = Path.GetFileNameWithoutExtension(projectBuilder.Args.EntryPoint.Value);
            entryPoint.TargetPath = Path.GetFileName(projectBuilder.Args.EntryPoint.Value);

            return entryPoint;
        }
    }
}
