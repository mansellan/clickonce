using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using ClickOnce.Resources;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

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

        internal static bool EqualsAny(this string value, params string[] compareTo) =>
            compareTo.Any(compare => compare.Equals(value));

        internal static ProcessorArchitecture PlatformConverter(string platform) =>
            platform?.ToLowerInvariant() switch
            {
                "anycpu" => ProcessorArchitecture.Msil,
                "msil" => ProcessorArchitecture.Msil,
                "x86" => ProcessorArchitecture.X86,
                "amd64" => ProcessorArchitecture.Amd64,
                "x64" => ProcessorArchitecture.Amd64,
                "itanium" => ProcessorArchitecture.Itanium,
                null => ProcessorArchitecture.Msil,
                _ => throw new InvalidEnumArgumentException(nameof(platform))
            };

        internal static string GetAttributeValue<TAttribute>(this Assembly assembly) => 
            assembly?
                .CustomAttributes
                .FirstOrDefault(ca => ca.AttributeType == typeof(TAttribute))?
                .ConstructorArguments
                .FirstOrDefault()
                .Value?
                .ToString()
                .EmptyToNull();

        internal static string GetFileInfo(string file, FileInfoKind kind)
        {
            if (file is null || !File.Exists(file))
                return null;

            var versionInfo = FileVersionInfo.GetVersionInfo(file);

            var ret = kind switch
            {
                FileInfoKind.ProductVersion => $"{versionInfo.ProductMajorPart}.{versionInfo.ProductMinorPart}.{versionInfo.ProductBuildPart}.{versionInfo.ProductPrivatePart}",
                FileInfoKind.CompanyName => versionInfo.CompanyName,
                FileInfoKind.FileDescription => versionInfo.FileDescription,
                _ => throw new InvalidEnumArgumentException(nameof(kind), (int) kind, typeof(FileInfoKind))
            };

            return string.IsNullOrWhiteSpace(ret) ? null : ret;
        }

        internal static bool Sign(string file, Project project)
        {
            if (project.CertificateSource.Value is null)
            {
                return false;
            }

            try
            {
                var certificateSource = project.CertificateSource.Value;
                var timestampUrl = project.TimestampUrl.Value is null ? null : new Uri(project.TimestampUrl.Value, UriKind.Absolute);

                if (certificateSource.Length == 40 && Regex.IsMatch(certificateSource, "[a-fA-F0-9]*"))
                {
                    SecurityUtilities.SignFile(certificateSource, timestampUrl, file);
                }
                else
                {
                    SecurityUtilities.SignFile(Path.Combine(project.Source.Value, project.CertificateSource.Value), project.CertificatePassword.Value, timestampUrl, file);
                }

                return true;
            }
            catch
            {
                throw new ApplicationException($"An error occurred signing the manifest. Check that all signature parameters are correct.");
            }
        }
    }
}

