using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommandLine;

namespace ClickOnce
{
    [Verb("create", HelpText = "Creates a ClickOnce package from scratch.")] // TODO
    public class CreateArgs : Args
    {
        public CreateArgs()
            : base(ArgsSource.CommandLine)
        {
        }

        [Option(HelpText = "Create_Source", ResourceType = typeof(HelpText))]
        public override string Source { get => base.Source;  set => base.Source = value; }

        [Option(HelpText = "Create_Target", ResourceType = typeof(HelpText))]
        public override string Target { get => base.Target; set => base.Target = value; }

        [Option(HelpText = "Create_Name", ResourceType = typeof(HelpText))]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Option(HelpText = "Create_Version", ResourceType = typeof(HelpText))]
        public override string Version { get => base.Version; set => base.Version = value; }

        [Option(HelpText = "Create_Suite", ResourceType = typeof(HelpText))]
        public override string Suite { get => base.Suite; set => base.Suite = value; }

        [Option(HelpText = "Create_Publisher", ResourceType = typeof(HelpText))]
        public override string Publisher { get => base.Publisher; set => base.Publisher = value; }

        [Option(HelpText = "Create_Description", ResourceType = typeof(HelpText))]
        public override string Description { get => base.Description; set => base.Description = value; }

        [Option(HelpText = "Create_EntryPoint", ResourceType = typeof(HelpText))]
        public override string EntryPoint { get => base.EntryPoint; set => base.EntryPoint = value; }

        [Option(HelpText = "Create_IconFile", ResourceType = typeof(HelpText))]
        public override string IconFile { get => base.IconFile; set => base.IconFile = value; }

        [Option(HelpText = "Create_Platform", ResourceType = typeof(HelpText))]
        public override Platform Platform { get => base.Platform; set => base.Platform = value; }

        [Option(HelpText = "Create_Culture", ResourceType = typeof(HelpText))]
        public override string Culture { get => base.Culture; set => base.Culture = value; }

        [Option(HelpText = "Create_OsVersion", ResourceType = typeof(HelpText))]
        public override string OsVersion { get => base.OsVersion; set => base.OsVersion = value; }

        [Option(HelpText = "Create_TargetFramework", ResourceType = typeof(HelpText))]
        public override string TargetFramework { get => base.TargetFramework; set => base.TargetFramework = value; }

        [Option(HelpText = "Create_Assemblies", Separator = '|', ResourceType = typeof(HelpText))]
        public override IEnumerable<string> Assemblies { get => base.Assemblies; set => base.Assemblies = value; }

        [Option(HelpText = "Create_Files", Separator = '|', ResourceType = typeof(HelpText))]
        public override IEnumerable<string> Files { get => base.Files; set => base.Files = value; }

        [Option(HelpText = "Create_DataFiles", Separator = '|', ResourceType = typeof(HelpText))]
        public override IEnumerable<string> DataFiles { get => base.DataFiles; set => base.DataFiles = value; }

        [Option(HelpText = "Create_DeploymentUrl", ResourceType = typeof(HelpText))]
        public override string DeploymentUrl { get => base.DeploymentUrl; set => base.DeploymentUrl = value; }

        [Option(HelpText = "Create_ErrorUrl", ResourceType = typeof(HelpText))]
        public override string ErrorUrl { get => base.ErrorUrl; set => base.ErrorUrl = value; }

        [Option(HelpText = "Create_SupportUrl", ResourceType = typeof(HelpText))]
        public override string SupportUrl { get => base.SupportUrl; set => base.SupportUrl = value; }

        [Option(HelpText = "Create_PackageMode", ResourceType = typeof(HelpText))]
        public override string PackageMode { get => base.PackageMode; set => base.PackageMode = value; }

        [Option(HelpText = "Create_LaunchMode", ResourceType = typeof(HelpText))]
        public override string LaunchMode { get => base.LaunchMode; set => base.LaunchMode = value; }

        [Option(HelpText = "Create_UpdateMode", ResourceType = typeof(HelpText))]
        public override string UpdateMode { get => base.UpdateMode; set => base.UpdateMode = value; }

        [Option(HelpText = "Create_MinimumVersion", ResourceType = typeof(HelpText))]
        public override string MinimumVersion { get => base.MinimumVersion; set => base.MinimumVersion = value; }

        [Option(HelpText = "Create_TrustUrlParameters", ResourceType = typeof(HelpText))]
        public override bool? TrustUrlParameters { get => base.TrustUrlParameters; set => base.TrustUrlParameters = value; }

        [Option(HelpText = "Create_UseDeployExtension", ResourceType = typeof(HelpText))]
        public override bool? UseDeployExtension { get => base.UseDeployExtension; set => base.UseDeployExtension = value; }

        [Option(HelpText = "Create_CreateDesktopShortcut", ResourceType = typeof(HelpText))]
        public override bool? CreateDesktopShortcut { get => base.CreateDesktopShortcut; set => base.CreateDesktopShortcut = value; }

        [Option(HelpText = "Create_UseApplicationTrust", ResourceType = typeof(HelpText))]
        public override bool? UseApplicationTrust { get => base.UseApplicationTrust; set => base.UseApplicationTrust = value; }

        [Option(HelpText = "Create_TrustInfo", ResourceType = typeof(HelpText))]
        public override string TrustInfo { get => base.TrustInfo; set => base.TrustInfo = value; }

        [Option(HelpText = "Create_CertificateSource", ResourceType = typeof(HelpText))]
        public override string CertificateSource { get => base.CertificateSource; set => base.CertificateSource = value; }

        [Option(HelpText = "Create_CertificatePassword", ResourceType = typeof(HelpText))]
        public override string CertificatePassword { get => base.CertificatePassword; set => base.CertificatePassword = value; }

        [Option(HelpText = "Create_Quiet", ResourceType = typeof(HelpText))]
        public override bool? Quiet { get => base.Quiet; set => base.Quiet = value; }

        [Option(HelpText = "Create_Verbose", ResourceType = typeof(HelpText))]
        public override bool? Verbose { get => base.Verbose; set => base.Verbose = value; }
    }
}
