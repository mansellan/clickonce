using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClickOnce
{
    public class GlobOption : Option<IEnumerable<string>>
    {
        private readonly string source;
        private readonly string target;
        private readonly string optionalFilesPath;

        public GlobOption(Option<IEnumerable<string>> option, GlobKind kind, string source, string target, string optionalFilesPath)
            : base(option.Source, option.Name, option.Value)
        {
            Kind = kind;
            this.source = source;
            this.target = target;
            this.optionalFilesPath = optionalFilesPath;
        }

        public GlobKind Kind { get; }

        public IEnumerable<string> ExpandRequired()
        {
            var patterns = Value.Concat(new[] { $"!{target}" }).ToArray();

            if (optionalFilesPath != null && !string.IsNullOrWhiteSpace(optionalFilesPath))
            {
                patterns = patterns.Concat(new[] {$"!{optionalFilesPath}"}).ToArray();
            }

            return Globber.Expand(source, patterns);
        }

        public IEnumerable<KeyValuePair<string, string>> ExpandOptional()
        {
            var ret = new List<KeyValuePair<string, string>>();

            if (optionalFilesPath == null || string.IsNullOrWhiteSpace(optionalFilesPath))
                return ret;

            var optionalFilesPathRooted = Path.Combine(source, optionalFilesPath);

            if (!Directory.Exists(optionalFilesPathRooted))
                return ret;

            foreach (var directory in Directory.GetDirectories(optionalFilesPathRooted))
            {
                var directoryName = new DirectoryInfo(directory).Name;

                foreach (var file in Globber.Expand(optionalFilesPathRooted, Value.ToArray()))
                {
                    ret.Add(new KeyValuePair<string, string>(directoryName, Path.Combine(optionalFilesPath, file)));
                }
            }

            return ret;
        }
    }
}
