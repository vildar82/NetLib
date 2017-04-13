using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.IO
{
    public class DllResolve
    {
        public DllResolve(string dllFile)
        {
            this.DllFile = dllFile;
            DllName = System.IO.Path.GetFileNameWithoutExtension(dllFile);
        }

        public string DllFile { get; set; }
        public string DllName { get; set; }        

        public bool IsResolve(string dllRequest)
        {
            return dllRequest.StartsWith($"{DllName},", StringComparison.OrdinalIgnoreCase);
        }

        public Assembly LoadAssembly()
        {
            return Assembly.LoadFrom(DllFile);
        }

        public static List<DllResolve> GetDllResolve(string dllFolder, SearchOption mode)
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
