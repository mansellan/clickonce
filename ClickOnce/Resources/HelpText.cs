using System.Resources;
using ClickOnce.Resources;

namespace ClickOnce
{
    public class HelpText
    {
        public static string Create_Source => GetString("Create", "Source");
        public static string Create_Target => GetString("Create", "Target");
        public static string Create_Name => GetString("Create", "Name");
        public static string Create_Version => GetString("Create", "Version");
        public static string Create_Suite => GetString("Create", "Suite");
        public static string Create_Publisher => GetString("Create", "Publisher");
        public static string Create_Description => GetString("Create", "Description");
        public static string Create_EntryPoint => GetString("Create", "EntryPoint");
        public static string Create_IconFile => GetString("Create", "IconFile");
        public static string Create_Platform => GetString("Create", "Platform");
        public static string Create_Culture => GetString("Create", "Culture");
        public static string Create_OsVersion => GetString("Create", "OsVersion");
        public static string Create_TargetFramework => GetString("Create", "TargetFramework");
        public static string Create_Assemblies => GetString("Create", "Assemblies");
        public static string Create_Files => GetString("Create", "Files");
        public static string Create_DataFiles => GetString("Create", "DataFiles");
        public static string Create_DeploymentUrl => GetString("Create", "DeploymentUrl");
        public static string Create_ErrorUrl => GetString("Create", "ErrorUrl");
        public static string Create_SupportUrl => GetString("Create", "SupportUrl");
        public static string Create_PackageMode => GetString("Create", "PackageMode");
        public static string Create_LaunchMode => GetString("Create", "LaunchMode");
        public static string Create_UpdateMode => GetString("Create", "UpdateMode");
        public static string Create_MinimumVersion => GetString("Create", "MinimumVersion");
        public static string Create_TrustUrlParameters => GetString("Create", "TrustUrlParameters");
        public static string Create_UseDeployExtension => GetString("Create", "UseDeployExtension");
        public static string Create_CreateDesktopShortcut => GetString("Create", "CreateDesktopShortcut");
        public static string Create_UseApplicationTrust => GetString("Create", "UseApplicationTrust");
        public static string Create_TrustInfo => GetString("Create", "TrustInfo");
        public static string Create_CertificateSource => GetString("Create", "CertificateSource");
        public static string Create_CertificatePassword => GetString("Create", "CertificatePassword");
        public static string Create_Quiet => GetString("Create", "Quiet");
        public static string Create_Verbose => GetString("Create", "Verbose");

        private static string GetString(string verb, string arg)
        {
            var value = Messages.ResourceManager.GetString($"Arg.{arg}") ?? string.Empty;

            value = $"{value} {Messages.ResourceManager.GetString($"{verb}.{arg}") ?? string.Empty}".Trim();
            
            return value.Length == 0 
                ? string.Format(Messages.ResourceManager.GetString("Error.HelpTextNotFound") ?? "Help text not found for '{0}'.", arg) 
                : value;
        }
    }
}
