using System.Collections.Generic;
using System.Linq;

namespace ClickOnce
{
    public class GlobOption : Option<IEnumerable<string>>
    {
        private readonly string source;
        private readonly string target;

        public GlobOption(Option<IEnumerable<string>> option, GlobKind kind, string source, string target)
            : base(option.Source, option.Name, option.Value)
        {
            Kind = kind;
            this.source = source;
            this.target = target;
        }

        public GlobKind Kind { get; }
        public IEnumerable<string> Expand() => Globber.Expand(source, Value.Concat(new[] {$"!{target}"}).ToArray());
    }
}
