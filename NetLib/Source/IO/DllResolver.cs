namespace NetLib.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Разрешение сборки
    /// </summary>
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

        public static List<DllResolve> GetDllResolve(string dllFolder, SearchOption mode)
        {
            return GetDllResolve(dllFolder, mode, ".dll");
        }

        /// <summary>
        /// Список резолвов
        /// </summary>
        /// <param name="dllFolder">Папка</param>
        /// <param name="mode">Режим</param>
        /// <param name="extFilter">С точкой!!!</param>
        /// <returns>Список</returns>
        public static List<DllResolve> GetDllResolve(string dllFolder, SearchOption mode, params string[] extFilter)
        {
            var dllResolves = new List<DllResolve>();
            foreach (var dllFile in Directory.EnumerateFiles(dllFolder, "*.*", mode).Where(f => IsFilterExt(f, extFilter)))
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

        public bool IsResolve(string dllRequest)
        {
            return dllRequest.StartsWith($"{DllName},", StringComparison.OrdinalIgnoreCase);
        }

        public Assembly LoadAssembly()
        {
            return Assembly.LoadFrom(DllFile);
        }
    }
}