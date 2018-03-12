using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace NetLib.Files
{
    public static class RobocopyExt
    {
        /// <summary>
        /// Копирование папки с зеркалированием. Содержимое папки назначения будет точно соответствовать папке источника.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <exception cref="Exception">StandardError</exception>
        /// <returns>Output</returns>
        [NotNull]
        public static string Mirror([NotNull] string sourceDir, [NotNull] string destDir)
        {
            if (!Directory.Exists(sourceDir)) throw new DirectoryNotFoundException(sourceDir);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            var startInfo = new ProcessStartInfo
            {
                FileName = "robocopy.exe",
                // R - число попыток
                // FP - Включать в вывод полные пути файлов.
                Arguments = $@"""{sourceDir}"" ""{destDir}"" /R:0 /MIR /FP",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var p = Process.Start(startInfo) ?? throw new InvalidOperationException();
            var err = p.StandardError.ReadToEnd();
            if (!err.IsNullOrEmpty())
            {
                throw new Exception(err);
            }
            return p.StandardOutput.ReadToEnd();
        }
    }
}
