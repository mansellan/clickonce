using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClickOnce
{
    internal class Project : Enumerable<Option>
    {
        private readonly IEnumerable<Args> args;

        internal Project(params Args[] args)
        {
            this.args = new List<Args>(args) { new InferredArgs(this) };
        }

        internal PathOption Source => args.GetPath();
        internal PathOption Target => args.GetPath();
        internal StringOption Identity => args.GetString();
        internal StringOption Product => args.GetString();
        internal VersionOption Version => args.GetVersion();
        internal VersionOption ApplicationVersion => args.GetVersion();
        internal StringOption Suite => args.GetString();
        internal StringOption Publisher => args.GetString();
        internal StringOption Description => args.GetString();
        internal PathOption EntryPoint => args.GetPath();
        internal PathOption IconFile => args.GetPath();
        internal StringOption DeploymentPage => args.GetString();
        internal PathOption PackagePath => args.GetPath(Target.RootedPath);
        internal PathOption ApplicationManifestFile => args.GetPath(PackagePath.RootedPath);
        internal PathOption DeploymentManifestFile => args.GetPath(Target.RootedPath);
        internal EnumOption<ProcessorArchitecture> ProcessorArchitecture => args.GetEnum(Utilities.PlatformConverter, "Platform");
        internal StringOption Culture => args.GetString();
        internal VersionOption OsVersion => args.GetVersion();
        internal StringOption OsDescription => args.GetString();
        internal StringOption OsSupportUrl => args.GetString();
        internal FrameworkOption TargetFramework => args.GetFramework();
        internal GlobOption Assemblies => args.GetGlob(GlobKind.Assemblies, Source.RootedPath, Target.Value);
        internal GlobOption Files => args.GetGlob(GlobKind.Files, Source.RootedPath, Target.Value);
        internal GlobOption DataFiles => args.GetGlob(GlobKind.DataFiles, Source.RootedPath, Target.Value);
        internal StringOption UpdateUrl => args.GetString();
        internal StringOption ErrorUrl => args.GetString();
        internal StringOption SupportUrl => args.GetString();
        internal EnumOption<PackageMode> PackageMode => args.GetEnum<PackageMode>();
        internal EnumOption<LaunchMode> LaunchMode => args.GetEnum<LaunchMode>();
        internal UpdateOption Update => args.GetUpdate("UpdateMode");
        internal VersionOption MinimumVersion => args.GetVersion();
        internal BooleanOption TrustUrlParameters => args.GetBoolean();
        internal BooleanOption UseDeployExtension => args.GetBoolean();
        internal EnumOption<UseLauncher> UseLauncher => args.GetEnum<UseLauncher>();
        internal BooleanOption CreateDesktopShortcut => args.GetBoolean();
        internal BooleanOption CreateAutoRun => args.GetBoolean();
        internal BooleanOption UseApplicationTrust => args.GetBoolean();
        //internal StringOption TrustInfo => args.GetString();
        //internal StringOption CertificateSource => args.GetString();
        //internal StringOption CertificatePassword => args.GetString();
        internal BooleanOption Quiet => args.GetBoolean();
        internal BooleanOption Verbose => args.GetBoolean();

        internal void Validate()
        {
            if (!Directory.Exists(Source.Value))
            {
                throw new ApplicationException($"'{Source}' does not exist.");
            }

            if (File.Exists(Target.RootedPath))
            {
                throw new ApplicationException($"'{Target}' is a file.");
            }

            if (EntryPoint.Value is null)
            {
                throw new ApplicationException("Entry point not specified, and could not be inferred.");
            }

            if (!File.Exists(EntryPoint.RootedPath))
            {
                throw new ApplicationException($"Entry point assembly '{EntryPoint}' not found.");
            }

            if (!(IconFile?.Value is null) && !File.Exists(IconFile.RootedPath))
            {
                throw new ApplicationException($"Icon file '{IconFile}' not found.");
            }

            if (TargetFramework.Value.EqualsAny("net20", "net30") && Update.Value.Enabled && UpdateUrl.Value is null)
            {
                throw new ApplicationException("For target frameworks prior to net35, UpdateUrl is required if update mode is not 'none'.");
            }
        }
    }
}
