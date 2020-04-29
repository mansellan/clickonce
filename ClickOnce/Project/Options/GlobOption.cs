using System.Collections.Generic;
using System.Linq;

namespace ClickOnce
{
    public class GlobOption : Option<IEnumerable<string>>
    {
        private readonly string source;
        private readonly IEnumerable<string> excludes;

        public GlobOption(Option<IEnumerable<string>> option, GlobKind kind, string source, string target, string entryPoint, string iconFile)
            : base(option.Source, option.Name, option.Value)
        {
            Kind = kind;
            this.source = source;
            excludes = new[]
            {
                $"!{target}",
                $"!{entryPoint}",
                $"!{iconFile}"
            };
        }

        public GlobKind Kind { get; }
        public IEnumerable<string> Expand() => Globber.Expand(source, Value.Concat(excludes));
    }
}
