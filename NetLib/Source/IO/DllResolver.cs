using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NetLib.IO
{
    public class DllResolve
    {
        public DllResolve(string dllFile)
        {
            DllFile = dllFile;
            DllName = System.IO.Path.GetFileNameWithoutExtension(dllFile);
        }

        public string DllFile { get; set; }
        public string DllName { get; set; }

        public bool IsResolve([NotNull] string dllRequest)
        {
            return dllRequest.StartsWith($"{DllName},", StringComparison.OrdinalIgnoreCase);
        }

        [NotNull]
        public Assembly LoadAssembly()
        {
            return Assembly.LoadFrom(DllFile);
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
    }
}
