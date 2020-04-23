using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// ReSharper disable UnusedMember.Global, ValueParameterNotUsed, MemberCanBePrivate.Global, VirtualMemberNeverOverridden.Global, UnusedAutoPropertyAccessor.Global, MemberCanBeProtected.Global, UnusedMemberHierarchy.Global
namespace ClickOnce
{
    public abstract class Args
    {
        protected Args(ArgsSource source)
        {
            ArgsSource = source;
        }

        public ArgsSource ArgsSource { get; }

        public virtual string Key => GetType().Name.Replace("Args", string.Empty).ToLowerInvariant();

        public virtual string Source { get => source; set => SetValue(value, ref source); }
        private string source;

        public virtual string Target { get => target; set => SetValue(value, ref target); }
        private string target;

        public virtual string Name { get => name; set => SetValue(value, ref name); }
        private string name;

        [RegularExpression(RegExPatterns.AssemblyVersionNumber, ErrorMessage = ConstraintMessages.AssemblyVersionNumber)]
        public virtual string Version { get => version; set => SetValue(value, ref version); }
        private string version;

        public virtual string Suite { get => suite; set => SetValue(value, ref suite); }
        private string suite;

        public virtual string Publisher { get => publisher; set => SetValue(value, ref publisher); }
        private string publisher;

        public virtual string Description { get => description; set => SetValue(value, ref description); }
        private string description;

        [RegularExpression(RegExPatterns.ExeFile, ErrorMessage = ConstraintMessages.ExeFile)]
        public virtual string EntryPoint { get => entryPoint; set => SetValue(value, ref entryPoint); }
        private string entryPoint;

        [RegularExpression(RegExPatterns.IconFile, ErrorMessage = ConstraintMessages.IconFile)]
        public virtual string IconFile { get => iconFile; set => SetValue(value, ref iconFile); }
        private string iconFile;

        public virtual string PackagePath { get => packagePath; set => SetValue(value, ref packagePath); }
        private string packagePath;

        public virtual string ApplicationManifestFile { get => applicationManifestFile; set => SetValue(value, ref applicationManifestFile); }
        private string applicationManifestFile;

        public virtual string DeploymentManifestFile { get => deploymentManifestFile; set => SetValue(value, ref deploymentManifestFile); }
        private string deploymentManifestFile;

        [RegularExpression(RegExPatterns.Platform, ErrorMessage = ConstraintMessages.Platform)]
        public virtual Platform Platform { get => platform; set => SetValue(value, ref platform); }
        private Platform platform;

        [RegularExpression(RegExPatterns.Culture, ErrorMessage = ConstraintMessages.Culture)]
        public virtual string Culture { get => culture; set => SetValue(value, ref culture); }
        private string culture;

        [RegularExpression(RegExPatterns.WindowsVersionNumber, ErrorMessage = ConstraintMessages.WindowsVersionNumber)]
        public virtual string OsVersion { get => osVersion; set => SetValue(value, ref osVersion); }
        private string osVersion;

        [RegularExpression(RegExPatterns.TargetFramework, ErrorMessage = ConstraintMessages.TargetFramework)]
        public virtual string TargetFramework { get => targetFramework; set => SetValue(value, ref targetFramework); }
        private string targetFramework;

        public virtual IEnumerable<string> Assemblies { get => assemblies; set => SetValue(value, ref assemblies); }
        private IEnumerable<string> assemblies;

        public virtual IEnumerable<string> Files { get => files; set => SetValue(value, ref files); }
        private IEnumerable<string> files;

        public virtual IEnumerable<string> DataFiles { get => dataFiles; set => SetValue(value, ref dataFiles); }
        private IEnumerable<string> dataFiles;

        // TODO!!
        public virtual string DeploymentUrl
        {
            get => deploymentUrl;
            set
            {
                deploymentUrl = value;
                if (deploymentUrl != null && !(Uri.IsWellFormedUriString(deploymentUrl, UriKind.Absolute) || Regex.IsMatch(deploymentUrl, RegExPatterns.UncPath)))
                {
                    throw new ApplicationException($"'{deploymentUrl}' is not a valid URL.");
                }
            }
        }
        private string deploymentUrl;

        // TODO!!
        public virtual string ErrorUrl
        {
            get => errorUrl;
            set
            {
                errorUrl = value;
                if (errorUrl != null && !(Uri.IsWellFormedUriString(errorUrl, UriKind.Absolute) || Regex.IsMatch(deploymentUrl, RegExPatterns.UncPath)))
                {
                    throw new ApplicationException($"'{errorUrl}' is not a valid URL.");
                }
            }
        }
        private string errorUrl;

        // TODO!!
        public virtual string SupportUrl
        {
            get => supportUrl;
            set
            {
                supportUrl = value;
                if (supportUrl != null && !(Uri.IsWellFormedUriString(supportUrl, UriKind.Absolute) || Regex.IsMatch(deploymentUrl, RegExPatterns.UncPath)))
                {
                    throw new ApplicationException($"'{supportUrl}' is not a valid URL.");
                }
            }
        }
        private string supportUrl;

        [RegularExpression(RegExPatterns.PackageMode, ErrorMessage = ConstraintMessages.PackageMode)]
        public virtual string PackageMode { get => packageMode; set => SetValue(value, ref packageMode); }
        private string packageMode;

        [RegularExpression(RegExPatterns.LaunchMode, ErrorMessage = ConstraintMessages.LaunchMode)]
        public virtual string LaunchMode { get => launchMode; set => SetValue(value, ref launchMode); }
        private string launchMode;

        [RegularExpression(RegExPatterns.UpdateMode, ErrorMessage = ConstraintMessages.UpdateMode)]
        public virtual string UpdateMode { get => updateMode; set => SetValue(value, ref updateMode); }
        private string updateMode;

        [RegularExpression(RegExPatterns.AssemblyVersionNumber, ErrorMessage = ConstraintMessages.AssemblyVersionNumber)]
        public virtual string MinimumVersion { get => minimumVersion; set => SetValue(value, ref minimumVersion); }
        private string minimumVersion;

        public virtual bool? TrustUrlParameters { get => trustUrlParameters; set => SetValue(value, ref trustUrlParameters); }
        private bool? trustUrlParameters;

        public virtual bool? UseDeployExtension { get => useDeployExtension; set => SetValue(value, ref useDeployExtension); }
        private bool? useDeployExtension;

        public virtual bool? CreateDesktopShortcut { get => createDesktopShortcut; set => SetValue(value, ref createDesktopShortcut); }
        private bool? createDesktopShortcut;

        public virtual bool? UseApplicationTrust { get => useApplicationTrust; set => SetValue(value, ref useApplicationTrust); }
        private bool? useApplicationTrust;

        public virtual string TrustInfo { get => trustInfo; set => SetValue(value, ref trustInfo); }
        private string trustInfo;

        public virtual string CertificateSource { get => certificateSource; set => SetValue(value, ref certificateSource); }
        private string certificateSource;

        public virtual string CertificatePassword { get => certificatePassword; set => SetValue(value, ref certificatePassword); }
        private string certificatePassword;

        public virtual bool? Quiet { get => quiet; set => SetValue(value, ref quiet); }
        private bool? quiet;

        public virtual bool? Verbose { get => verbose; set => SetValue(value, ref verbose); }
        private bool? verbose;

        protected void SetValue<T>(T value, ref T backingField, [CallerMemberName] string caller = null)
        {
            if (caller is null) return;

            Validator.ValidateProperty(value, new ValidationContext(this) { MemberName = caller });

            backingField = value;
        }
    }

    public class Platform
    {
        public Platform(string raw)
        {
            Raw = raw;
            Parsed = raw == null ? null : raw.ToLowerInvariant() switch
            {
                "anycpu" => "msil",
                "msil" => "msil",
                "x86" => "x86",
                "x64" => "amd64",
                "amd64" => "amd64",
                "Itanium" => "itanium",
                _ => null
            };
        }

        public string Raw { get; }
        public string Parsed { get; }
        public override string ToString() => Raw;
    }

    public enum ArgsSource { CommandLine, ProjectFile, Settings, Inferred, Default, Internal }
}
