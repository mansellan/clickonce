using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace ClickOnce
{
    internal static class LauncherBuilder
    {
        internal static string Build(Project project)
        {
            Directory.CreateDirectory(project.PackagePath.RootedPath);

            var peKind = project.ProcessorArchitecture.Value switch
            {
                ProcessorArchitecture.Msil => PortableExecutableKinds.ILOnly,
                ProcessorArchitecture.X86 => PortableExecutableKinds.Required32Bit,
                ProcessorArchitecture.Amd64 => PortableExecutableKinds.PE32Plus,
                ProcessorArchitecture.Itanium => PortableExecutableKinds.PE32Plus,
                _ => PortableExecutableKinds.ILOnly
            };

            var imageFileMachine = project.ProcessorArchitecture.Value switch
            {
                ProcessorArchitecture.Msil => ImageFileMachine.I386,
                ProcessorArchitecture.X86 => ImageFileMachine.I386,
                ProcessorArchitecture.Amd64 => ImageFileMachine.AMD64,
                ProcessorArchitecture.Itanium => ImageFileMachine.IA64,
                _ => ImageFileMachine.I386
            };

            var exeName = project.EntryPoint?.Value;
            var launcherName = $"{Path.GetFileNameWithoutExtension(exeName)}.Launcher";
            var launcherExe = $"{launcherName}.exe";

            var start = typeof(Process).GetMethod("Start", new[] { typeof(string) });
            var builder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(launcherName), AssemblyBuilderAccess.Save, project.PackagePath.RootedPath);
            var module = builder.DefineDynamicModule(launcherName, launcherExe);
            var type = module.DefineType("Launcher", TypeAttributes.Public);
            var method = type.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static, null, null);

            builder.SetEntryPoint(method, PEFileKinds.WindowApplication);
            builder.DefineVersionInfoResource(project.Product.Value, project.Version.Value, project.Publisher.Value, null, null);

            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldstr, exeName);
            il.Emit(OpCodes.Call, start);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);

            type.CreateType();

            if (peKind == PortableExecutableKinds.ILOnly)
            {
                builder.Save(launcherExe);
            }
            else
            {
                builder.Save(launcherExe, peKind, imageFileMachine);
            }

            return Path.Combine(project.PackagePath.RootedPath, launcherExe);
        }
    }
}
