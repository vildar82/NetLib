using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Path = NetLib.IO.Path;

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
            const string logFile = "robocopy.log";
            var startInfo = new ProcessStartInfo
            {
                FileName = "robocopy.exe",
                // R - число попыток
                // FP - Включать в вывод полные пути файлов.
                Arguments = $@"""{sourceDir}"" ""{destDir}"" /R:0 /MIR /FP /LOG:{logFile}",
                ErrorDialog = false,
                LoadUserProfile = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                //RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var p = Process.Start(startInfo) ?? throw new InvalidOperationException())
            {
                p.WaitForExit();
                var err = p.StandardError.ReadToEnd();
                if (!err.IsNullOrEmpty())
                {
                    throw new Exception(err);
                }
                var res = File.ReadAllText(logFile, Encoding.GetEncoding(866));
                Path.TryDeleteFile(logFile);
                return res;
            }
        }
    }
}
