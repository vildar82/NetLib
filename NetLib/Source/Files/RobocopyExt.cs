// ReSharper disable once CheckNamespace
namespace NetLib.Files
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using JetBrains.Annotations;
    using Path = NetLib.IO.Path;

    public static class RobocopyExt
    {
        public static string Mirror(string sourceDir, string destDir, bool showConsole = false)
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
        public static string Mirror(string sourceDir, string destDir, out int exitCode, bool showConsole = false)
        {
            exitCode = 0;
            if (!Directory.Exists(sourceDir)) throw new DirectoryNotFoundException(sourceDir);
            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
            var logFile = "robocopy.log";
            var startInfo = new ProcessStartInfo
            {
                FileName = "robocopy.exe",
                // R - число попыток
                // FP - Включать в вывод полные пути файлов.
                Arguments = $@"""{sourceDir}"" ""{destDir}"" /R:0 /MIR /FP /TEE /LOG:""{logFile}""",
                ErrorDialog = false,
                LoadUserProfile = false,
                UseShellExecute = showConsole,
                CreateNoWindow = !showConsole,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true
            };

            using (var p = Process.Start(startInfo) ?? throw new InvalidOperationException())
            {
                p.WaitForExit();
                exitCode = p.ExitCode;
                //var err = p.StandardError.ReadToEnd();
                //if (!err.IsNullOrEmpty())
                //{
                //    throw new Exception(err);
                //}
            }

            try
            {
                var res = File.ReadAllText(logFile, Encoding.GetEncoding(866));
                Path.TryDeleteFile(logFile);
                return res;
            }
            catch
            {
                return "";
            }
        }
    }
}
