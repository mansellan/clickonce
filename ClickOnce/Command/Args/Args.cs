using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ClickOnce.Resources;
using CommandLine;

// ReSharper disable UnusedMember.Global, ValueParameterNotUsed, MemberCanBePrivate.Global, VirtualMemberNeverOverridden.Global, UnusedAutoPropertyAccessor.Global, MemberCanBeProtected.Global, UnusedMemberHierarchy.Global
namespace ClickOnce
{
    public abstract class Args
    {
        protected Args(ArgsSource source, CommandVerb? verb = null)
        {
            ArgsSource = source;
            Verb = verb;
        }

        public ArgsSource ArgsSource { get; }
        public static CommandVerb? Verb { get; private set; }

        public virtual string Key => GetType().Name.Replace("Args", string.Empty);

        [Option(HelpText = nameof(Help_Arg_Source), ResourceType = typeof(Args))]
        public virtual string Source { get => source; set => SetValue(value, ref source); }
        private string source;
        public static string Help_Arg_Source => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Target), ResourceType = typeof(Args))]
        public virtual string Target { get => target; set => SetValue(value, ref target); }
        private string target;
        public static string Help_Arg_Target => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Name), ResourceType = typeof(Args))]
        public virtual string Name { get => name; set => SetValue(value, ref name); }
        private string name;
        public static string Help_Arg_Name => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Version), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.AssemblyVersionNumber, ErrorMessageResourceName = nameof(Messages.Help_Arg_Version_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string Version { get => version; set => SetValue(value, ref version); }
        private string version;
        public static string Help_Arg_Version => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Suite), ResourceType = typeof(Args))]
        public virtual string Suite { get => suite; set => SetValue(value, ref suite); }
        private string suite;
        public static string Help_Arg_Suite => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Publisher), ResourceType = typeof(Args))]
        public virtual string Publisher { get => publisher; set => SetValue(value, ref publisher); }
        private string publisher;
        public static string Help_Arg_Publisher => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Description), ResourceType = typeof(Args))]
        public virtual string Description { get => description; set => SetValue(value, ref description); }
        private string description;
        public static string Help_Arg_Description => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_EntryPoint), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.ExeFile, ErrorMessageResourceName = nameof(Messages.Help_Arg_EntryPoint_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string EntryPoint { get => entryPoint; set => SetValue(value, ref entryPoint); }
        private string entryPoint;
        public static string Help_Arg_EntryPoint => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_IconFile), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.IconFile, ErrorMessageResourceName = nameof(Messages.Help_Arg_IconFile_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string IconFile { get => iconFile; set => SetValue(value, ref iconFile); }
        private string iconFile;
        public static string Help_Arg_IconFile => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_PackagePath), ResourceType = typeof(Args))]
        public virtual string PackagePath { get => packagePath; set => SetValue(value, ref packagePath); }
        private string packagePath;
        public static string Help_Arg_PackagePath => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_ApplicationManifestFile), ResourceType = typeof(Args))]
        public virtual string ApplicationManifestFile { get => applicationManifestFile; set => SetValue(value, ref applicationManifestFile); }
        private string applicationManifestFile;
        public static string Help_Arg_ApplicationManifestFile => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_DeploymentManifestFile), ResourceType = typeof(Args))]
        public virtual string DeploymentManifestFile { get => deploymentManifestFile; set => SetValue(value, ref deploymentManifestFile); }
        private string deploymentManifestFile;
        public static string Help_Arg_DeploymentManifestFile => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Platform), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.Platform, ErrorMessageResourceName = nameof(Messages.Help_Arg_Platform_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string Platform { get => platform; set => SetValue(value, ref platform); }
        private string platform;
        public static string Help_Arg_Platform => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Culture), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.Culture, ErrorMessageResourceName = nameof(Messages.Help_Arg_Culture_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string Culture { get => culture; set => SetValue(value, ref culture); }
        private string culture;
        public static string Help_Arg_Culture => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_OsVersion), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.WindowsVersionNumber, ErrorMessageResourceName = nameof(Messages.Help_Arg_OsVersion_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string OsVersion { get => osVersion; set => SetValue(value, ref osVersion); }
        private string osVersion;
        public static string Help_Arg_OsVersion => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_OsDescription), ResourceType = typeof(Args))]
        public virtual string OsDescription { get => osDescription; set =>SetValue(value, ref osDescription); }
        private string osDescription;
        public static string Help_Arg_OsDescription => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_OsSupportUrl), ResourceType = typeof(Args))]
        [Uri]
        public virtual string OsSupportUrl { get => osSupportUrl; set => SetValue(value, ref osSupportUrl); }
        private string osSupportUrl;
        public static string Help_Arg_OsSupportUrl => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_TargetFramework), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.TargetFramework, ErrorMessageResourceName = nameof(Messages.Help_Arg_TargetFramework_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string TargetFramework { get => targetFramework; set => SetValue(value, ref targetFramework); }
        private string targetFramework;
        public static string Help_Arg_TargetFramework => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Assemblies), ResourceType = typeof(Args))]
        public virtual IEnumerable<string> Assemblies { get => assemblies; set => SetValue(value, ref assemblies); }
        private IEnumerable<string> assemblies;
        public static string Help_Arg_Assemblies => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_Files), ResourceType = typeof(Args))]
        public virtual IEnumerable<string> Files { get => files; set => SetValue(value, ref files); }
        private IEnumerable<string> files;
        public static string Help_Arg_Files => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_DataFiles), ResourceType = typeof(Args))]
        public virtual IEnumerable<string> DataFiles { get => dataFiles; set => SetValue(value, ref dataFiles); }
        private IEnumerable<string> dataFiles;
        public static string Help_Arg_DataFiles => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_DeploymentUrl), ResourceType = typeof(Args))]
        [Uri]
        public virtual string DeploymentUrl { get => deploymentUrl; set => SetValue(value, ref deploymentUrl); }
        private string deploymentUrl;
        public static string Help_Arg_DeploymentUrl => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_ErrorUrl), ResourceType = typeof(Args))]
        [Uri]
        public virtual string ErrorUrl { get => errorUrl; set => SetValue(value, ref errorUrl); }
        private string errorUrl;
        public static string Help_Arg_ErrorUrl => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_SupportUrl), ResourceType = typeof(Args))]
        [Uri]
        public virtual string SupportUrl { get => supportUrl; set => SetValue(value, ref supportUrl); }
        private string supportUrl;
        public static string Help_Arg_SupportUrl => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_PackageMode), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.PackageMode, ErrorMessageResourceName = nameof(Messages.Help_Arg_PackageMode_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string PackageMode { get => packageMode; set => SetValue(value, ref packageMode); }
        private string packageMode;
        public static string Help_Arg_PackageMode => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_LaunchMode), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.LaunchMode, ErrorMessageResourceName = nameof(Messages.Help_Arg_LaunchMode_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string LaunchMode { get => launchMode; set => SetValue(value, ref launchMode); }
        private string launchMode;
        public static string Help_Arg_LaunchMode => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_UpdateMode), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.UpdateMode, ErrorMessageResourceName = nameof(Messages.Help_Arg_UpdateMode_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string UpdateMode { get => updateMode; set => SetValue(value, ref updateMode); }
        private string updateMode;
        public static string Help_Arg_UpdateMode => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_MinimumVersion), ResourceType = typeof(Args))]
        [RegularExpression(RegExPatterns.AssemblyVersionNumber, ErrorMessageResourceName = nameof(Messages.Help_Arg_MinimumVersion_Constraint), ErrorMessageResourceType = typeof(Messages))]
        public virtual string MinimumVersion { get => minimumVersion; set => SetValue(value, ref minimumVersion); }
        private string minimumVersion;
        public static string Help_Arg_MinimumVersion => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_TrustUrlParameters), ResourceType = typeof(Args))]
        public virtual bool? TrustUrlParameters { get => trustUrlParameters; set => SetValue(value, ref trustUrlParameters); }
        private bool? trustUrlParameters;
        public static string Help_Arg_TrustUrlParameters => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_UseDeployExtension), ResourceType = typeof(Args))]
        public virtual bool? UseDeployExtension { get => useDeployExtension; set => SetValue(value, ref useDeployExtension); }
        private bool? useDeployExtension;
        public static string Help_Arg_UseDeployExtension => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_CreateDesktopShortcut), ResourceType = typeof(Args))]
        public virtual bool? CreateDesktopShortcut { get => createDesktopShortcut; set => SetValue(value, ref createDesktopShortcut); }
        private bool? createDesktopShortcut;
        public static string Help_Arg_CreateDesktopShortcut => GetHelpText();

        [Option(HelpText = nameof(Help_Arg_UseApplicationTrust), ResourceType = typeof(Args))]
        public virtual bool? UseApplicationTrust { get => useApplicationTrust; set => SetValue(value, ref useApplicationTrust); }
        private bool? useApplicationTrust;
        public static string Help_Arg_UseApplicationTrust => GetHelpText();

        //[Option(HelpText = nameof(Help_Arg_TrustInfo), ResourceType = typeof(Args))]
        //public virtual string TrustInfo { get => trustInfo; set => SetValue(value, ref trustInfo); }
        //private string trustInfo;
        //public static string Help_Arg_TrustInfo => GetHelpText();

        //[Option(HelpText = nameof(Help_Arg_CertificateSource), ResourceType = typeof(Args))]
        //public virtual string CertificateSource { get => certificateSource; set => SetValue(value, ref certificateSource); }
        //private string certificateSource;
        //public static string Help_Arg_CertificateSource => GetHelpText();

        //[Option(HelpText = nameof(Help_Arg_CertificatePassword), ResourceType = typeof(Args))]
        //public virtual string CertificatePassword { get => certificatePassword; set => SetValue(value, ref certificatePassword); }
        //private string certificatePassword;
        //public static string Help_Arg_CertificatePassword => GetHelpText();

        [Option(SetName = "Quiet", HelpText = nameof(Help_Arg_Quiet), ResourceType = typeof(Args))]
        public virtual bool? Quiet { get => quiet; set => SetValue(value, ref quiet); }
        private bool? quiet;
        public static string Help_Arg_Quiet => GetHelpText();

        [Option(SetName = "Verbose", HelpText = nameof(Help_Arg_Verbose), ResourceType = typeof(Args))]
        public virtual bool? Verbose { get => verbose; set => SetValue(value, ref verbose); }
        private bool? verbose;
        public static string Help_Arg_Verbose => GetHelpText();

        protected void SetValue<T>(T value, ref T backingField, [CallerMemberName] string caller = null)
        {
            if (caller is null) return;

            Validator.ValidateProperty(value, new ValidationContext(this) { MemberName = caller });

            backingField = value;
        }

        private static string GetHelpText([CallerMemberName] string name = null)
        {
            if (name is null)
                return Messages.Build_Exceptions_HelpText_NotFound;

            name = name.Replace('_', '.');
            var value = Utilities.GetMessage(name) ?? string.Empty;
            value = $"{value} {Utilities.GetMessage($"{name}.Constraint") ?? string.Empty}".Trim();
            value = $"{value} {Utilities.GetMessage($"{name}.{Verb}") ?? string.Empty}".Trim();

            return value.Length == 0
                ? Messages.Build_Exceptions_HelpText_NotFound
                : value;
        }
    }
}
