using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NetLib.IO
{
    using System.Linq;

    /// <summary>
    /// Разрешение сборки
    /// </summary>
    [PublicAPI]
    [ToString]
    public class DllResolve
    {
        public string DllName { get; set; }
        
        public string DllFile { get; set; }

        public DllResolve(string dllFile)
        {
            DllFile = dllFile;
            DllName = System.IO.Path.GetFileNameWithoutExtension(dllFile);
        }

        [NotNull]
        public static List<DllResolve> GetDllResolve([NotNull] string dllFolder, SearchOption mode)
        {
            return GetDllResolve(dllFolder, mode, "dll");
        }

        [NotNull]
        public static List<DllResolve> GetDllResolve([NotNull] string dllFolder, SearchOption mode, params string[] extFilter)
        {
            var dllResolves = new List<DllResolve>();
            foreach (var dllFile in Directory.EnumerateFiles(dllFolder, "*", mode).Where(f => IsFilterExt(f, extFilter)))
            {
                var dllResolve = new DllResolve(dllFile);
                dllResolves.Add(dllResolve);
            }
            return dllResolves;
        }

        private static bool IsFilterExt(string file, string[] extFilter)
        {
            var ext = System.IO.Path.GetExtension(file);
            return extFilter.Any(a => a.EqualsIgnoreCase(ext));
        }

        public bool IsResolve([NotNull] string dllRequest)
        {
            return dllRequest.StartsWith($"{DllName},", StringComparison.OrdinalIgnoreCase);
        }

        [NotNull]
        public Assembly LoadAssembly()
        {
            return Assembly.LoadFrom(DllFile);
        }
    }
}