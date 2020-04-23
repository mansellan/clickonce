using System.Collections.Generic;
using System.Linq;

namespace ClickOnce
{
    internal static class RegExPatterns
    {
        internal const string ExeFile = @"^[\w\-.][\w\-. ]*\.[eE][xX][eE]$";
        internal const string IconFile = @"^[\w\-.][\w\-. ]*\.[iI][cC][oO]$";
        internal const string Platform = @"^[aA][nN][yY][cC][pP][uU]|[xX]86|[xX]64|[iI][tT][aA][nN][iI][uU][mM]$";
        internal const string AssemblyVersionNumber = @"^(?:\d{1,4}|[0-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-4])(?:\.(?:\d{1,4}|[0-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-4])){0,3}$";
        internal const string WindowsVersionNumber = @"^(?:4\.0|4\.1|4\.9|5\.0|5\.1|5\.2|6\.0|6\.1|6\.2|6\.3|10\.0)(?:\.\d{1,5}(?:\.\d{1,5})?)?$";
        internal const string Culture = @"^[nN][eE][uU][tT][rR][aA][lL]|[a-zA-Z]{2}-[a-zA-Z]{2}$";
        internal const string TargetFramework = @"^[nN][oO][nN][eE]|[nN][eE][tT](?:20|30|35|40|403|45|451|452|46|461|462|47|471|472|48\d?)$";
        internal const string PackageMode = @"[aA][pP][pP][lL][iI][cC][aA][tT][iI][oO][nN]|[dD][eE][pP][lL][oO][yY][mM][eE][nN][tT]|[bB][oO][tT][hH]";
        internal const string LaunchMode = @"^[sS][tT][aA][rR][tT]|[uU][rR][lL]|[bB][oO][tT][hH]$";
        internal const string UpdateMode = @"^(?:[oO][fF][fF]|[sS][tT][aA][rR][tT](?:[iI][nN][gG]|[eE][dD])|(?:\d{1,3}|[1-7]\d{3}|8[0-6]\d{2}|87[0-5]\d|8760)h|(?:\d{1,2}|[1-2]\d{2}|3[0-5]\d|36[0-5])d|(?:|\d|[1-4]\d|5[0-2])w)$";
        internal const string UncPath = @"^\\(?:\\[^\r\n\t<>:""|? *\\\/]+){2,}\\?$";
    }

    internal static class ConstraintMessages
    {
        internal const string ExeFile = "Must be an .exe file.";
        internal const string IconFile = "Must be an .ico file.";
        internal const string Platform = "Must be one of 'AnyCPU', 'x86', 'x64', 'Itanium'.";
        internal const string Culture = "Must be 'neutral' or a valid culture (e.g. 'en-GB')";
        internal const string AssemblyVersionNumber = "Must be a dotted version number with 1 to 4 elements, each less than 63356.";
        internal const string WindowsVersionNumber = "Must be a dotted version number with 2 to 4 elements. The first 2 elements must match a known Windows version (e.g. '6.0' for Windows Vista).";
        internal const string TargetFramework = "Must be either 'none' or a net framework from 'net20' onwards. If 'none' is specified, a bootstrapper will be created to launch the application.";
        internal const string PackageMode = "Must be one of 'application', 'deployment', 'both'.";
        internal const string LaunchMode = "Must be one of 'start', 'url', 'both'.";
        internal const string UpdateMode = "Must be one of 'off', 'starting', 'started', or a number of hours, weeks or days (e.g. '1w'). Only one unit can be specifed, and the interval cannot describe more than 1 year, regardless of unit.";
    }

    internal static class GlobPatterns
    {
        private const string AssemblyExtensions = "dll,exe";
        private const string FileExtensions = "config,manifest,ico,bmp,jpeg,jpg,gif,txt,md,xml,json";
        private const string DataFileExtensions = "user,dat";

        internal static readonly IEnumerable<string> Assemblies = Get(AssemblyExtensions);
        internal static readonly IEnumerable<string> Files = Get(FileExtensions);
        internal static readonly IEnumerable<string> DataFiles = Get(DataFileExtensions);

        private static IEnumerable<string> Get(string extensions) => extensions.Split(',').Select(ext => $"**/*.{ext}").ToHashSet();
    }
}
