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
        private readonly Lazy<Assembly> entryPointAssembly;

        public InferredArgs(params Args[] args)
            : base(ArgsSource.Inferred)
        {
            project = new Project(args);
            entryPoint = new Lazy<string>(GetEntryPoint);
            entryPointAssembly = new Lazy<Assembly>(GetEntryPointAssembly);
        }

        public override string Source => AppDomain.CurrentDomain.BaseDirectory;

        public override string Name => entryPointAssembly.Value?.GetName().Name;

        public override string Version => entryPointAssembly.Value?.GetName().Version.ToString();

        public override string Publisher => entryPointAssembly.Value?.CustomAttributes.FirstOrDefault(ca => ca.AttributeType == typeof(AssemblyCompanyAttribute))?.ConstructorArguments.FirstOrDefault().Value?.ToString().EmptyToNull()
            ?? entryPointAssembly.Value?.GetName().Name;

        public override string Description => entryPointAssembly.Value?.CustomAttributes.FirstOrDefault(ca => ca.AttributeType == typeof(AssemblyDescriptionAttribute))?.ConstructorArguments.FirstOrDefault().Value?.ToString().EmptyToNull();

        public override string EntryPoint => entryPoint.Value;

        public override string PackagePath => Path.Combine("Application Files", $"{Path.GetFileNameWithoutExtension(EntryPoint)}_{(project.Version?.Value ?? Version).Replace('.', '_')}");

        public override string ApplicationManifestFile => Path.GetFileName(EntryPoint) + ".manifest";

        public override string DeploymentManifestFile => Path.GetFileNameWithoutExtension(EntryPoint) + ".application";

        public override Platform Platform
        {
            get
            {
                var processorArchitecture = entryPointAssembly.Value?.GetName().ProcessorArchitecture.ToString();
                return processorArchitecture == null ? null : new Platform(processorArchitecture);
            }
        }

        public override string OsVersion => null; // args.InferOsVersion();

        public override string TargetFramework => null;

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
            var candidates = Globber.Expand(project.Source.RootedPath ?? Source,  new[] { "**\\*.exe" }, new[] { project.Target.Value }).ToArray();

            switch (candidates.Length)
            {
                case 0:
                    throw new ApplicationException(Messages.Build_Exceptions_NoEntryPoint);

                case 1:
                    return candidates.Single();

                default:
                    throw new ApplicationException(Messages.Build_Exceptions_MultipleEntryPoints);
            }
        }
    }
}
