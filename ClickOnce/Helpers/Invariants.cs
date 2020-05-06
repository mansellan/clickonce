using System;
using System.Collections.Generic;

namespace ClickOnce
{
    public enum CommandVerb
    {
        Create,
        Configure
    }

    public enum ArgsSource
    {
        CommandLine, 
        ProjectFile, 
        Settings, 
        Inferred, 
        Default, 
        Internal
    }

    internal enum LogLevel
    {
        Verbose = 1,
        Normal = 2,
        Quiet = 3
    }

    public enum GlobKind
    {
        Assemblies,
        Files,
        DataFiles
    }

    [Flags]
    public enum PackageMode
    {
        None = 0,
        Application = 1,
        Deployment = 2,
        Both = 3
    }

    [Flags]
    public enum LaunchMode
    {
        Start = 1,
        Url = 2,
        Both = 3,
        Browser = 6 // Only works in IE :-)
    }

    public enum ProcessorArchitecture
    {
        Msil,
        X86,
        Amd64,
        Itanium
    }

    public enum UseBootstrapper
    {
        True,
        False,
        Auto
    }

    public enum FileInfoKind
    {
        ProductVersion,
        CompanyName,
        FileDescription
    }

    internal static class RegExPatterns
    {
        internal const string ExeFile = @"^[\w\-.][\w\-. ]*\.[eE][xX][eE]$";
        internal const string IconFile = @"^[\w\-.][\w\-. ]*\.[iI][cC][oO]$";
        internal const string Platform = @"^[aA][nN][yY][cC][pP][uU]|[xX]86|[xX]64|[iI][tT][aA][nN][iI][uU][mM]$";
        internal const string AssemblyVersionNumber = @"^(?:\d{1,4}|[0-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-4])(?:\.(?:\d{1,4}|[0-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-4])){0,3}$";
        internal const string WindowsVersionNumber = @"^(?:4\.0|4\.1|4\.9|5\.0|5\.1|5\.2|6\.0|6\.1|6\.2|6\.3|10\.0)(?:\.\d{1,5}(?:\.\d{1,5})?)?$";
        internal const string Culture = @"^[nN][eE][uU][tT][rR][aA][lL]|[a-zA-Z]{2}-[a-zA-Z]{2}$";
        internal const string TargetFramework = @"^[nN][oO][nN][eE]|[nN][eE][tT](?:20|30|35|40|403|45|451|452|46|461|462|47|471|472|48\d?)$";
        internal const string PackageMode = @"[nN][oO][nN][eE]|[aA][pP][pP][lL][iI][cC][aA][tT][iI][oO][nN]|[dD][eE][pP][lL][oO][yY][mM][eE][nN][tT]|[bB][oO][tT][hH]";
        internal const string LaunchMode = @"^[sS][tT][aA][rR][tT]|[uU][rR][lL]|[bB][oO][tT][hH]|[bB][rR][oO][wW][sS][eE][rR]$";
        internal const string UpdateMode = @"^(?:[nN][oO][nN][eE]|[sS][tT][aA][rR][tT](?:[iI][nN][gG]|[eE][dD])|(?:\d{1,3}|[1-7]\d{3}|8[0-6]\d{2}|87[0-5]\d|8760)h|(?:\d{1,2}|[1-2]\d{2}|3[0-5]\d|36[0-5])d|(?:|\d|[1-4]\d|5[0-2])w)$";
        internal const string UseBootstrapper = @"^[tT][rR][uU][eE]|[fF][aA][lL][sS][eE]|[aA][uU][tT][oO]$";
    }

    internal static class WindowsVersions
    {
        private static readonly Dictionary<string, string> Versions = new Dictionary<string, string>
        {
            {"4.10", "Windows 98"},
            {"4.90", "Windows Millennium" },
            {"5.0", "Windows 2000" },
            {"5.1", "Windows XP" },
            {"6.0", "Windows Vista" },
            {"6.1", "Windows 7" },
            {"6.2", "Windows 8" },
            {"6.3", "Windows 8.1" },
            {"10.0", "Windows 10" }
        };

        public static string GetDescription(string value)
        {
            if (value is null)
                return null;

            var elements = value.Split('.');
            if (elements.Length < 2)
                return null;

            return Versions.TryGetValue($"{elements[0]}.{elements[1]}", out var description)
                ? description
                : null;
        }
    }

    public static class DotNetFrameworks
    {
        private static readonly Dictionary<string, string> Map = new Dictionary<string, string>
        {
            {"net20", "4.10.1998"},
            {"net30", "5.1.2600.2180"},
            {"net35", "5.1.2600.2180"},
            {"net40", "5.1.2600"},
            {"net45", "6.0.6002"},
            {"net451", "6.0.6002"},
            {"net452", "6.0.6002"},
            {"net46", "6.0.6002"},
            {"net461", "6.1.7601"},
            {"net462", "6.1.7601"},
            {"net47", "6.1.7601"},
            {"net471", "6.1.7601"},
            {"net472", "6.1.7601"},
            {"net48", "6.1.7601"},
        };

        public static string GetMinimumOsVersion(string framework) =>
            framework is null
                ? null
                : Map.TryGetValue(framework.ToLowerInvariant(), out var osVersion)
                    ? osVersion
                    : null;
    }
}
