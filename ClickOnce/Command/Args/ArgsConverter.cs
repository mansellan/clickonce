using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ClickOnce.Resources;

namespace ClickOnce
{
    public static class ArgsConverter
    {
        private static Option<T> Get<T>(this IEnumerable<Args> args, Func<string, T> converter = null, [CallerMemberName] string name = null)
        {
            foreach (var next in args)
            {
                var property = next
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));

                if (property is null)
                {
                    throw new ArgumentException(string.Format(Messages.Build_Exceptions_Arg_NotFound, name));
                }

                var value = property.GetValue(next);
                if (value is null || value is IEnumerable<string> enumerable && !enumerable.Any())
                    continue;

                if (converter is null)
                {
                    // Target type directly assignable?
                    if (value is T parsed)
                    {
                        return new Option<T>(next.ArgsSource, name, parsed, value);
                    }
                    throw new ArgumentException(string.Format(Messages.Build_Exceptions_Arg_NotConvertible, name, value , typeof(T).Name));
                }

                return new Option<T>(next.ArgsSource, name, converter(value.ToString()), value);
            }

            return new Option<T>(ArgsSource.Internal, name, default);
        }

        public static StringOption GetString(this IEnumerable<Args> args, Func<string, string> converter = null, [CallerMemberName] string name = null) =>
            new StringOption(args.Get(converter, name));

        public static BooleanOption GetBoolean(this IEnumerable<Args> args, [CallerMemberName] string name = null) =>
            new BooleanOption(args.Get<bool>(null, name));

        public static UpdateOption GetUpdate(this IEnumerable<Args> args, [CallerMemberName] string name = null) =>
            new UpdateOption(args.Get<string>(null, name));

        public static VersionOption GetVersion(this IEnumerable<Args> args, [CallerMemberName] string name = null) =>
            new VersionOption(args.Get<string>(null, name));

        public static FrameworkOption GetFramework(this IEnumerable<Args> args, [CallerMemberName] string name = null) =>
            new FrameworkOption(args.Get<string>(null, name));

        public static PathOption GetPath(this IEnumerable<Args> args, string basePath = null, [CallerMemberName] string name = null)
        {
            basePath ??= name.Equals("source", StringComparison.InvariantCultureIgnoreCase)
                ? Path.GetDirectoryName(Directory.GetCurrentDirectory())
                : args.GetPath(null, "source").RootedPath;

            var path = args.Get<string>(null, name);

            if (path?.Value is null || basePath is null)
                return new PathOption(path, null);

            var rootedPath =
                Path.IsPathRooted(path.Value)
                    ? path.Value
                    : Path.Combine(basePath, path.Value);

            return new PathOption(path, rootedPath);
        }

        public static EnumOption<T> GetEnum<T>(this IEnumerable<Args> args, Func<string, T> converter = null, [CallerMemberName] string name = null) where T : struct, Enum =>
            new EnumOption<T>(args.Get<string>(null, name), converter);

        public static GlobOption GetGlob(this IEnumerable<Args> args, GlobKind kind, string source, string target, string optionalFilesPath, [CallerMemberName] string name = null) =>
            new GlobOption(args.Get<IEnumerable<string>>(null, name), kind, source, target, optionalFilesPath);

        public static SecureOption GetSecure(this IEnumerable<Args> args, [CallerMemberName] string name = null) =>
            new SecureOption(args.Get<string>(null, name));

        public static TrustOption GetTrust(this IEnumerable<Args> args, bool sameSite, string sourcePath, [CallerMemberName] string name = null) =>
            new TrustOption(args.Get<string>(null, name), sameSite, sourcePath);
    }
}
