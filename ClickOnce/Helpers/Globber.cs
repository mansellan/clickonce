using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace ClickOnce
{
    internal static class Globber
    {

        internal static IEnumerable<string> Expand(string source, IEnumerable<string> patterns)
        {
            if (patterns is null || !patterns.Any())
                return Enumerable.Empty<string>();
            
            var files = new List<string>();
            var matcher = new Matcher();

            foreach (var pattern in patterns)
            {
                if (pattern.StartsWith("!"))
                {
                    matcher.AddExclude(pattern.Substring(1));
                }
                else
                {
                    matcher.AddInclude(pattern);
                }
            }

            var results = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(source)));
            files.AddRange(results.Files.Select(file => file.Stem.Replace('/', '\\')));

            return files;
        }
    }
}
