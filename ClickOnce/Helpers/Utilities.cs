
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;

namespace ClickOnce
{


    internal static class Utilities
    {
        //internal static TEnum Parse<TEnum>(string value) where TEnum : Enum
        //    => (TEnum)Enum.Parse(typeof(TEnum), value);

        //public static string SplitPascalCase(this string text)
        //    => string.IsNullOrEmpty(text)
        //        ? text 
        //        : Regex.Replace(text, "([A-Z])", " $1", RegexOptions.Compiled).Trim();

        internal static string EmptyToNull(this string value) =>
            string.IsNullOrEmpty(value)
                ? null
                : value;

        internal static void ResolveGlobs(this Manifest application, ProjectBuilder projectBuilder)
        {
            
            application.AddAssembly(projectBuilder, projectBuilder.Args.EntryPoint.RootedPath, projectBuilder.Args.EntryPoint.Value);
            application.AddAssemblies(projectBuilder);
            application.AddFiles(projectBuilder.Args.Files.Value, false, projectBuilder);
            application.AddFiles(projectBuilder.Args.DataFiles.Value, true, projectBuilder);

            CopyFile(projectBuilder.Args.IconFile.RootedPath, projectBuilder);
        }

        private static void AddAssemblies(this Manifest application, ProjectBuilder projectBuilder)
        {
            foreach (var assembly in projectBuilder.Args.Assemblies.Value)
            {
                // TODO: Filter out unmanaged

                var source = Path.Combine(projectBuilder.Args.Source.Value, assembly);

                application.AssemblyReferences.Add(new AssemblyReference
                {
                    SourcePath = source,
                    TargetPath = assembly,
                    AssemblyIdentity = AssemblyIdentity.FromFile(source)
                });

                CopyFile(source, projectBuilder);
            }
        }

        private static void AddAssembly(this Manifest application, ProjectBuilder projectBuilder, string source, string target)
        {
            application.AssemblyReferences.Add(new AssemblyReference
            {
                SourcePath = source,
                TargetPath = target,
                AssemblyIdentity = AssemblyIdentity.FromFile(source)
            });

            CopyFile(source, projectBuilder);
        }

        private static void AddFiles(this Manifest application, IEnumerable<string> files, bool isDataFile, ProjectBuilder projectBuilder)
        {
            foreach (var file in files)
            {
                var source = Path.Combine(projectBuilder.Args.Source.Value, file);
                var target = Path.Combine(projectBuilder.Args.PackagePath.RootedPath, file);

                var fileReference = new FileReference
                {
                    SourcePath = source,
                    TargetPath = target,
                    IsDataFile = isDataFile
                };

                application.FileReferences.Add(fileReference);
                CopyFile(source, projectBuilder);
            }
        }

        private static void CopyFile(string source, ProjectBuilder projectBuilder)
        {
            if (source is null) return;

            var target = Path.Combine(projectBuilder.Args.PackagePath.RootedPath, Path.GetFileName(source));
            Directory.CreateDirectory(Path.GetDirectoryName(target));
            File.Copy(source, target, true);
        }
    }
}
