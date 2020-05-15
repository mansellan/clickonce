using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ClickOnce.Resources;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace ClickOnce
{
    internal class InferredArgs : Args
    {
        private readonly Project project;
        private readonly Lazy<string> iconFile;
        private readonly Lazy<string> entryPoint;
        private readonly Lazy<Assembly> entryPointAssembly;
        private readonly Lazy<AssemblyIdentity> entryPointIdentity;

        public InferredArgs(Project project)
            : base(ArgsSource.Inferred)
        {
            this.project = project;
            iconFile = new Lazy<string>(GetIconFile);
            entryPoint = new Lazy<string>(GetEntryPoint);
            entryPointAssembly = new Lazy<Assembly>(GetEntryAssembly);
            entryPointIdentity = new Lazy<AssemblyIdentity>(GetEntryIdentity);
        }

        public override string Source => Directory.GetCurrentDirectory();

        public override string Target => "publish";

        public override string Identity => Path.GetFileNameWithoutExtension(entryPoint.Value);

        public override string Product => project.Identity?.Value ?? Identity;

        public override string Version =>
            entryPointAssembly.Value?.GetAttributeValue<AssemblyVersionAttribute>()
            ?? entryPointIdentity.Value?.Version
            ?? Utilities.GetFileInfo(project.EntryPoint?.RootedPath, FileInfoKind.ProductVersion);

        public override string Publisher =>
            entryPointAssembly.Value?.GetAttributeValue<AssemblyCompanyAttribute>()
            ?? Utilities.GetFileInfo(project.EntryPoint?.RootedPath, FileInfoKind.CompanyName)
            ?? Identity;

        public override string Description => 
            entryPointAssembly.Value?.GetAttributeValue<AssemblyDescriptionAttribute>()
            ?? Utilities.GetFileInfo(project.EntryPoint?.RootedPath, FileInfoKind.FileDescription);

        public override string EntryPoint => entryPoint.Value;

        public override string IconFile => iconFile.Value;

        public override string PackagePath => Path.Combine("Application Files", $"{(project.Identity?.Value ?? Path.GetFileNameWithoutExtension(EntryPoint))}_{(project.Version?.Value ?? Version).Replace('.', '_')}");

        public override string ApplicationManifestFile => (project.Identity?.Value ?? Identity) + ".exe.manifest";

        public override string DeploymentManifestFile => (project.Identity?.Value ?? Identity) + ".application";

        public override string Platform =>
            (entryPointAssembly.Value?.GetName().ProcessorArchitecture.ToString()
             ?? entryPointIdentity.Value?.ProcessorArchitecture)?.ToLowerInvariant()
                switch
                {
                    "msil" => "AnyCPU",
                    "x86" => "x86",
                    "amd64" => "x64",
                    "ia64" => "Itanium",
                    _ => null
                };

        public override string Culture =>
            entryPointAssembly.Value?.GetAttributeValue<AssemblyCultureAttribute>() 
            ?? entryPointIdentity.Value?.Culture
            ?? "neutral";

        public override string OsVersion => DotNetFrameworks.GetMinimumOsVersion(project.TargetFramework.Value ?? TargetFramework);

        public override string OsDescription => WindowsVersions.GetDescription(project.OsVersion.Value ?? OsVersion);

        public override string TargetFramework => "net472";

        public override string PackageMode => "both";

        public override string LaunchMode => "both";

        public override string UpdateMode => project.TargetFramework?.Value?.EqualsAny("net20", "net30") ?? false ? "none" : "starting";

        public override string MinimumVersion => project.Update.Value?.Enabled ?? false ? project.Version.Value : null;

        public override string UseLauncher => "auto";

        public override bool? UseDeployExtension => true;

        public override bool? TrustUrlParameters => true;

        public override bool? CreateDesktopShortcut => false;

        public override bool? CreateAutoRun => false;

        private string GetEntryPoint()
        {
            var candidates = Globber.Expand(project.Source?.RootedPath ?? Source, "**/*.exe", $"!{project.Target?.Value ?? Target}").ToArray();

            switch (candidates.Length)
            {
                case 0:
                    throw new ApplicationException(Messages.Build_Exceptions_EntryPoint_None);

                case 1:
                    return candidates.Single();

                default:
                    throw new ApplicationException(Messages.Build_Exceptions_EntryPoint_Multiple);
            }
        }

        private Assembly GetEntryAssembly()
        {
            if (project.EntryPoint?.RootedPath is null)
                return null;

            try
            {
                 return Assembly.LoadFrom(project.EntryPoint.RootedPath);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private AssemblyIdentity GetEntryIdentity()
        {
            if (project.EntryPoint?.RootedPath is null)
                return null;

            try
            {
                var identity = AssemblyIdentity.FromFile(project.EntryPoint.RootedPath);

                if (!(identity is null))
                    return identity;

                // Could be a netcoreapp. Look for a dll with the same name
                var dllFile = Path.Combine(Path.GetDirectoryName(project.EntryPoint.RootedPath), Path.GetFileNameWithoutExtension(project.EntryPoint.RootedPath) + ".dll");

                return File.Exists(dllFile) 
                    ? AssemblyIdentity.FromFile(dllFile) 
                    : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetIconFile()
        {
            var candidates = Globber.Expand(project.Source?.RootedPath ?? Source, "**/*.ico", $"!{project.Target?.Value}").ToArray();

            return candidates.Length == 1
                ? candidates[0]
                : null;
        }
    }
}
