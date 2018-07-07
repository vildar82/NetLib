using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NetLib.IO
{
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
            var dllResolves = new List<DllResolve>();
            foreach (var dllFile in Directory.EnumerateFiles(dllFolder, "*.dll", mode))
            {
                var dllResolve = new DllResolve(dllFile);
                dllResolves.Add(dllResolve);
            }
            return dllResolves;
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