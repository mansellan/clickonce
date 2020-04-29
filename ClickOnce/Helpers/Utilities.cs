using System.ComponentModel;
using System.Text.RegularExpressions;
using ClickOnce.Resources;

namespace ClickOnce
{
    internal static class Utilities
    {
        public static string SplitPascalCase(this string text)
            => string.IsNullOrEmpty(text)
                ? text
                : Regex.Replace(text, "([A-Z])", " $1", RegexOptions.Compiled).Trim();

        internal static string GetMessage(string name) => Messages.ResourceManager.GetString(name);

        internal static string EmptyToNull(this string value) =>
            string.IsNullOrEmpty(value)
                ? null
                : value;

        internal static ProcessorArchitecture PlatformConverter(string platform) =>
            platform.ToLowerInvariant() switch
            {
                "anycpu" => ProcessorArchitecture.Msil,
                "msil" => ProcessorArchitecture.Msil,
                "x86" => ProcessorArchitecture.X86,
                "amd64" => ProcessorArchitecture.Amd64,
                "x64" => ProcessorArchitecture.Amd64,
                "itanium" => ProcessorArchitecture.Itanium,
                _ => throw new InvalidEnumArgumentException(nameof(platform))
            };
    }
}

