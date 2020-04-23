using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClickOnce.Resources;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace ClickOnce
{
    internal static class Globber
    {
        internal static IEnumerable<string> ExpandAssemblies(this Args args) => Enumerable.Empty<string>();
            //ExpandGlobs(args, args.Assemblies, "Assemblies");

            internal static IEnumerable<string> ExpandFiles(this Args args, bool dataFiles) => Enumerable.Empty<string>();
            //dataFiles 
            //    ? ExpandGlobs(args, args.DataFiles, "DataFiles") 
            //    : ExpandGlobs(args, args.Files, "Files");

        internal static IEnumerable<string> Expand(string source, IEnumerable<string> includes, IEnumerable<string> excludes, string loggerKey = null) //, bool excludeEntryPoint = true)
        {
            var files = new List<string>();
            var matcher = new Matcher();
            excludes ??= Enumerable.Empty<string>();

            if (includes is null || !includes.Any())
                return Enumerable.Empty<string>();

            foreach (var include in includes)
            {
                matcher.AddInclude(include);
            }

            foreach (var exclude in excludes)
            {
                if (exclude is null)
                    continue;

                matcher.AddExclude(exclude);
            }

            //AddExcludeRelative(args.IconFile);
            //AddExcludeRelative(args.Target);
            //if (excludeEntryPoint)
            //{
            //    AddExcludeRelative(args.EntryPoint);
            //}

            if (!(loggerKey is null))
                Logger.Normal(Messages.ResourceManager.GetString($"Glob.{loggerKey}"));

            var results = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(source)));

            if (!results.HasMatches)
            {
                Logger.Normal($"  {Messages.Glob_NoResults}");
            }
            else
            {
                files.AddRange(results.Files.Select(file => file.Stem.Replace('/', '\\')));

                if (!(loggerKey is null))
                {
                    foreach (var file in files)
                    {
                        Logger.Normal($"  {file}");
                    }
                }
            }

            if (!(loggerKey is null))
                Logger.Normal();

            return files;

            void AddExcludeRelative(string path) // TODO - not sure if we need to limit this to relative only...
            {
                if (path != null && !Path.IsPathRooted(path))
                {
                    matcher.AddExclude(path);
                }
            }
        }
    }
}
