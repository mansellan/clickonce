using System;
using System.IO;
using ClickOnce.Resources;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace ClickOnce
{
    internal static class ApplicationBuilder
    {
        internal static void Build(Project project)
        {
            var application = new ApplicationManifest
            {
                IsClickOnceManifest = true,
                ReadOnly = false,
                IconFile = project.IconFile.Value,
                OSVersion =  project.OsVersion.Value,
                OSDescription = project.OsDescription.Value,
                OSSupportUrl = project.OsSupportUrl.Value,
                TargetFrameworkVersion = project.TargetFramework.Version,
                HostInBrowser = project.LaunchMode.Value == LaunchMode.Browser,
                AssemblyIdentity = new AssemblyIdentity
                {
                    Name = Path.GetFileName(project.EntryPoint.Value),
                    Version = project.Version.Value,
                    Culture = project.Culture.Value,
                    ProcessorArchitecture = project.ProcessorArchitecture.Value.ToString().ToLowerInvariant()
                },
                UseApplicationTrust = project.UseApplicationTrust.Value,
                TrustInfo = new TrustInfo()
            };

            if (project.UseApplicationTrust.Value)
            {
                application.Publisher = project.Publisher.Value;
                application.SuiteName = project.Suite.Value;
                application.Product = project.Product.Value;
                application.SupportUrl = project.SupportUrl.Value;
                application.ErrorReportUrl = project.ErrorUrl.Value;
                application.Description = project.Description.Value;
            }

            application.AddEntryPoint(project);
            application.AddIconFile(project);
            application.AddGlob(project, project.Assemblies);
            application.AddGlob(project, project.Files);
            application.AddGlob(project, project.DataFiles);

            application.ResolveFiles();
            application.UpdateFileInfo(project.TargetFramework.Version);

            Logger.Normal(Messages.Build_Process_Application);
            application.Validate();
            Logger.OutputMessages(application.OutputMessages, 1);

            if (!project.PackageMode.Value.HasFlag(PackageMode.Application))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(project.ApplicationManifestFile.RootedPath));
            ManifestWriter.WriteManifest(application, project.ApplicationManifestFile.RootedPath, project.TargetFramework.Version);
            Logger.Normal(Messages.Build_Process_Manifest, 1, 2, project.ApplicationManifestFile.RootedPath);

        }

        private static void AddGlob(this ApplicationManifest application, Project project, GlobOption glob)
        {
            var any = false;
            Logger.Normal(Messages.Build_Process_Glob_Adding, 0, 1, glob.Kind.ToString().SplitPascalCase().ToLowerInvariant());
            foreach (var item in glob.Expand())
            {
                any |= application.Add(project, Path.Combine(project.Source.Value, item), item, glob.Kind);
            }
            if (!any)
            {
                Logger.Normal(Messages.Result_NoneFound, 1);
            }
            Logger.Normal();
        }

        private static void AddEntryPoint(this ApplicationManifest application, Project project)
        {
            Logger.Normal(Messages.Build_Process_EntryPoint);

            var assembly = project.EntryPoint.RootedPath;
            var assemblyIdentity = AssemblyIdentity.FromManagedAssembly(assembly);

            if (assemblyIdentity is null || project.UseLauncher.Value == UseLauncher.True)
            {
                if (project.UseLauncher.Value == UseLauncher.False)
                {
                    throw new ApplicationException(string.Format(Messages.Build_Exceptions_EntryPoint_NotManaged, assembly));
                }

                Logger.Normal(string.Format(Messages.Build_Process_Launcher, assembly), 1);
                assembly = LauncherBuilder.Build(project);
                assemblyIdentity = AssemblyIdentity.FromManagedAssembly(assembly);
            }

            if (assemblyIdentity is null)
            {
                throw new ApplicationException(Messages.Build_Exceptions_EntryPoint_Failed);
            }

            var assemblyReference = new AssemblyReference(assembly.Replace(".deploy", ""))
            {
                AssemblyIdentity = assemblyIdentity,
                TargetPath = Path.GetFileName(assembly.Replace(".deploy", ""))
            };

            if (!application.Add(project, assembly, assemblyReference.TargetPath, GlobKind.Assemblies))
            {
                throw new ApplicationException(Messages.Build_Exceptions_EntryPoint_Failed);
            }

            application.EntryPoint = assemblyReference;

            Logger.Normal();
        }

        private static void AddIconFile(this ApplicationManifest application, Project project)
        {
            Logger.Normal(Messages.Build_Process_IconFile);
            if (!application.Add(project, project.IconFile?.RootedPath, project.IconFile?.Value, GlobKind.Files))
            {
                Logger.Normal(Messages.Result_NoneFound, 1);
            }
            Logger.Normal();
        }

        private static bool Add(this Manifest application, Project project, string source, string target, GlobKind kind)
        {
            if (source is null || target is null)
                return false;

            if (kind == GlobKind.Assemblies 
                && application.AssemblyReferences.FindTargetPath(target) is null 
                && application.FileReferences.FindTargetPath(target) is null)
            {
                var identity = AssemblyIdentity.FromManagedAssembly(source);

                if (identity is null)
                {
                    Logger.Verbose(Messages.Build_Process_Glob_Skipped, 1, 1, source);
                    return false;
                }

                application.AssemblyReferences.Add(new AssemblyReference
                {
                    SourcePath = source,
                    TargetPath = target,
                    AssemblyIdentity = AssemblyIdentity.FromFile(source)
            });
            }
            else if (application.AssemblyReferences.FindTargetPath(target) is null 
                     && application.FileReferences.FindTargetPath(target) is null)
            {
                application.FileReferences.Add(new FileReference
                {
                    SourcePath = source,
                    TargetPath = target,
                    IsDataFile = kind == GlobKind.DataFiles
                });
            }
            else
            {
                return false;
            }

            CopyFile(source, target, project);
            Logger.Normal(source, 1);
            return true;
        }

        private static void CopyFile(string source, string target, Project project)
        {
            if (source is null) return;

            target = Path.Combine(project.PackagePath.RootedPath, target);

            if (project.UseDeployExtension.Value)
            {
                target += ".deploy";
            }
            if (source.Equals(target, StringComparison.InvariantCultureIgnoreCase)) return;

            Directory.CreateDirectory(Path.GetDirectoryName(target));
            File.Copy(source, target, true);
        }
    }
}
