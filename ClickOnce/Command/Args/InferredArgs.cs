using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ClickOnce.Resources;

namespace ClickOnce
{
    internal class InferredArgs : Args
    {
        private readonly Project project;
        private readonly Lazy<string> entryPoint;
        private readonly Lazy<string> iconFile;
        private readonly Lazy<Assembly> entryPointAssembly;

        public InferredArgs(params Args[] args)
            : base(ArgsSource.Inferred)
        {
            project = new Project(args);
            entryPoint = new Lazy<string>(GetEntryPoint);
            iconFile = new Lazy<string>(GetIconFile);
            entryPointAssembly = new Lazy<Assembly>(GetEntryPointAssembly);
        }

        public override string Source => AppDomain.CurrentDomain.BaseDirectory;

        public override string Target => "publish";

        public override string Name => entryPointAssembly.Value?.GetName().Name;

        public override string Version => entryPointAssembly.Value?.GetName().Version.ToString();

        public override string Publisher => GetAssemblyAttribute<AssemblyCompanyAttribute>() ?? entryPointAssembly.Value?.GetName().Name;

        public override string Description => GetAssemblyAttribute<AssemblyDescriptionAttribute>();

        public override string EntryPoint => entryPoint.Value;

        public override string IconFile => iconFile.Value;

        public override string PackagePath => Path.Combine("Application Files", $"{Path.GetFileNameWithoutExtension(EntryPoint)}_{(project.Version?.Value ?? Version).Replace('.', '_')}");

        public override string ApplicationManifestFile => Path.GetFileName(EntryPoint) + ".manifest";

        public override string DeploymentManifestFile => Path.GetFileNameWithoutExtension(EntryPoint) + ".application";

        public override string Platform =>
            entryPointAssembly.Value?.GetName().ProcessorArchitecture switch
            {
                System.Reflection.ProcessorArchitecture.MSIL => "AnyCPU",
                System.Reflection.ProcessorArchitecture.X86 => "x86",
                System.Reflection.ProcessorArchitecture.Amd64 => "x64",
                System.Reflection.ProcessorArchitecture.IA64 => "Itanium",
                _ => null
            };

        public override string Culture => GetAssemblyAttribute<AssemblyCultureAttribute>() ?? "neutral";

        public override string OsVersion => DotNetFrameworks.GetMinimumOsVersion(project.TargetFramework.Value ?? TargetFramework);

        public override string OsDescription => WindowsVersions.GetDescription(project.OsVersion.Value ?? OsVersion);

        public override string TargetFramework => "net472";

        public override string PackageMode => "both";

        public override string LaunchMode => "both";

        public override string UpdateMode => "off";

        public override string MinimumVersion => project.Update.Value?.Enabled ?? false ? null : project.Version.Value;

        private Assembly GetEntryPointAssembly()
        {
            if (entryPoint.Value is null)
            {
                return null;
            }

            return Assembly.ReflectionOnlyLoadFrom(Path.Combine(project.Source.Value ?? Source, entryPoint.Value));
        }

        private string GetEntryPoint()
        {
            if (!(project.EntryPoint.Value is null))
            {
                return project.EntryPoint.Value;
            }

            var candidates = Globber.Expand(project.Source.RootedPath ?? Source,  new[] { "**/*.exe", $"!{project.Target.Value}" }).ToArray();

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

        private string GetIconFile()
        {
            var candidates = Globber.Expand(project.Source.RootedPath ?? Source, new[] {"**/*.ico", $"!{project.Target.Value}"}).ToArray();

            return candidates.Length == 1
                ? candidates[0]
                : null;
        }

        private string GetAssemblyAttribute<T>() =>
            entryPointAssembly
                .Value?
                .CustomAttributes
                .FirstOrDefault(ca => ca.AttributeType == typeof(T))?
                .ConstructorArguments
                .FirstOrDefault()
                .Value?
                .ToString()
                .EmptyToNull();
    }
}
