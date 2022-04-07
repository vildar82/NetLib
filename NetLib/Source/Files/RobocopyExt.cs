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
        [NotNull]
        public static string Mirror([NotNull] string sourceDir, [NotNull] string destDir, bool showConsole = false)
        {
            return Mirror(sourceDir, destDir, out _, showConsole);
        }

        /// <summary>
        /// Копирование папки с зеркалированием. Содержимое папки назначения будет точно соответствовать папке источника.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="exitCode"></param>
        /// <param name="showConsole"></param>
        /// <exception cref="Exception">StandardError</exception>
        /// <returns>Output</returns>
        [NotNull]
        public static string Mirror(
            [NotNull] string sourceDir,
            [NotNull] string destDir,
            out int exitCode,
            bool showConsole = false)
        {
            exitCode = 0;
            if (!Directory.Exists(sourceDir)) throw new DirectoryNotFoundException(sourceDir);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            var startInfo = new ProcessStartInfo
            {
                FileName = "robocopy.exe",

                // R - число попыток
                // FP - Включать в вывод полные пути файлов.
                Arguments = $@"""{sourceDir}"" ""{destDir}"" /R:0 /MIR",
                ErrorDialog = false,
                LoadUserProfile = false,
                UseShellExecute = showConsole,
                CreateNoWindow = !showConsole,
            };

            using (var p = Process.Start(startInfo) ?? throw new InvalidOperationException())
            {
                p.WaitForExit();
                exitCode = p.ExitCode;
            }

            return exitCode.ToString();
        }
    }
}
