using System;
using System.Collections.Generic;
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

        public override string Identity => Path.GetFileNameWithoutExtension(project.EntryPoint.RootedPath);

        public override string Product => project.Identity?.Value ?? Identity;

        public override string Version => ApplicationVersion;

        public override string ApplicationVersion =>
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

        public override IEnumerable<string> Assemblies => new[] {"**/*.exe", "**/*.dll"};
        
        public override IEnumerable<string> DataFiles => new[] { "**/*.mdb" };
        
        public override IEnumerable<string> Files => new[] { "**/*" };

        public new string PrerequisitesLocation => "Vendor";

        public override string OptionalFilesPath => "Optional";

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
            ?? "Neutral";

        public override string OsVersion => DotNetFrameworks.GetMinimumOsVersion(project.TargetFramework.Value ?? TargetFramework);

        public override string OsDescription => WindowsVersions.GetDescription(project.OsVersion.Value ?? OsVersion);

        public override string TargetFramework => "net472";

        public override string PackageMode => "Both";

        public override string LaunchMode => "Start";

        public override string UpdateMode => project.TargetFramework?.Value?.EqualsAny("net20", "net30") ?? false ? "None" : "Starting";

        public override string MinimumVersion => project.Update.Value?.Enabled ?? false ? project.Version.Value : null;

        public override string UseLauncher => "Auto";

        public override bool? UseDeployExtension => true;

        public override string TrustInfo => "Full";

        public override bool? SameSite => true;

        public override bool? TrustUrlParameters => project.LaunchMode.Value.HasFlag(ClickOnce.LaunchMode.Url);

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
