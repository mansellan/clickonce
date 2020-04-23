using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ClickOnce
{
    internal class Project : IEnumerable<Option>
    {
        private readonly IEnumerable<Args> args;
        private readonly IList<Func<Option>> meta = new List<Func<Option>>();

        internal Project(params Args[] args)
        {
            this.args = args;
 
            foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                meta.Add(() => property.GetValue(this) as Option);
            }
        }

        internal PathOption Source => GetPath();
        internal PathOption Target => GetPath();
        internal StringOption Name => GetString();
        internal StringOption Version => GetString();
        internal StringOption Suite => GetString();
        internal StringOption Publisher => GetString();
        internal StringOption Description => GetString();
        internal PathOption EntryPoint => GetPath();
        internal PathOption IconFile => GetPath();
        internal PathOption PackagePath => GetPath(Target.RootedPath);
        internal PathOption ApplicationManifestFile => GetPath();
        internal PathOption DeploymentManifestFile => GetPath();
        internal StringOption ProcessorArchitecture
        {
            get
            {
                var arg = Get<Platform>("Platform");
                return new StringOption(arg.Source, arg.Name, arg.Value.Parsed);
            }
        }
        internal StringOption Culture => GetString();
        internal StringOption OsVersion => GetString();
        internal StringOption TargetFrameworkMoniker
        {
            get
            {
                var arg = GetString("TargetFramework");
                return new StringOption(arg.Source, arg.Name, $".NET Framework, Version=v{string.Join(".", arg.Value.Substring(3).ToCharArray())}");

            }
        }
        internal Option<IEnumerable<string>> Assemblies => Get<IEnumerable<string>>(); // new Option<IEnumerable<string>>(Assemblies,  Globber.Expand(Source.Value, Get<IEnumerable<string>>().Value, new[] { EntryPoint.Value });
        internal Option<IEnumerable<string>> Files => Get<IEnumerable<string>>();
        internal Option<IEnumerable<string>> DataFiles => Get<IEnumerable<string>>();
        internal StringOption DeploymentUrl => GetString();
        internal StringOption ErrorUrl => GetString();
        internal StringOption SupportUrl => GetString();
        internal StringOption PackageMode => GetString();
        internal StringOption LaunchMode => GetString();
        internal StringOption UpdateMode => GetString();
        internal StringOption MinimumVersion => GetString();
        internal Option<bool> TrustUrlParameters => Get<bool>();
        internal Option<bool> UseDeployExtension => Get<bool>();
        internal Option<bool> CreateDesktopShortcut => Get<bool>();
        internal Option<bool> UseApplicationTrust => Get<bool>();
        internal StringOption TrustInfo => GetString();
        internal StringOption CertificateSource => GetString();
        internal StringOption CertificatePassword => GetString();
        internal Option<bool> Quiet => Get<bool>();
        internal Option<bool> Verbose => Get<bool>();

        private Option<T> Get<T>([CallerMemberName] string name = null)
        {
            foreach (var next in args)
            {
                var property = next
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));

                if (property is null)
                    continue;

                var ret = property.GetValue(next);
                if (ret is IEnumerable<string> multi && !multi.Any())
                {
                    continue;
                }

                if (!(ret is null))
                {
                    return new Option<T>(next.ArgsSource, name, (T)ret);
                }
            }

            return new Option<T>(ArgsSource.Internal, name, default);
        }

        private StringOption GetString([CallerMemberName] string name = null) => new StringOption(Get<string>(name));
        

        private PathOption GetPath(string basePath = null, [CallerMemberName] string name = null)
        {
            basePath ??= name.Equals("source", StringComparison.InvariantCultureIgnoreCase)
                ? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                : Source.RootedPath;

            var path = Get<string>(name);

            var rootedPath = path?.Value is null
                ? null
                : Path.IsPathRooted(path.Value)
                    ? path.Value
                    : Path.Combine(basePath, path.Value);

            return new PathOption(path, rootedPath);
        }

        public IEnumerator<Option> GetEnumerator() => meta.Select(func => func.Invoke()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
