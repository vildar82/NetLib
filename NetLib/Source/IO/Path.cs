using JetBrains.Annotations;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using NetLib.Date;
using NLog;

namespace NetLib.IO
{
    [PublicAPI]
    public static class Path
    {
        private static ILogger Log { get; } = LogManager.GetCurrentClassLogger();

        public static void StartExplorer([NotNull] this string path)
        {
            if (path.IsFilePath())
            {
                var arg = "/select, \"" + path + "\"";
                System.Diagnostics.Process.Start("explorer.exe", arg);
            }
            else
            {
                System.Diagnostics.Process.Start(path);
            }

        }

        public static bool IsFilePath([NotNull] this string path)
        {
            var attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) != FileAttributes.Directory;
        }

        public static string GetFolderName(string dirPath)
        {
            return System.IO.Path.GetFileName(dirPath.TrimEnd(System.IO.Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// Files the exists with timeout.
        /// </summary>
        /// <param name="file">The file.</param>
        public static bool FileExists([NotNull] string file)
        {
            var task = Task.Run(() => File.Exists(file));
            task.Wait(TimeSpan.FromMilliseconds(1000));
            return task.IsCompleted && task.Result;
        }

        [NotNull]
        public static Task<bool> FileExistsAsync([NotNull] string file)
        {
            return Task.Run(() => File.Exists(file));
        }

        /// <summary>
        /// Gets the file thumbnail by Shell.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Image</returns>
        public static Bitmap GetThumbnail(string file)
        {
            var shellFile = ShellFile.FromFilePath(file);
            return shellFile.Thumbnail.ExtraLargeBitmap;
        }

        public static bool IsEqualsDateFile([NotNull] string file1, [NotNull] string file2)
        {
            var f1Date =File.GetLastWriteTime(file1);
            var f2Date = File.GetLastWriteTime(file2);
            return f1Date.IsEquals(f2Date);
        }

        /// <summary>
        /// Determines whether /[is newest file] [the specified file1].
        /// </summary>
        /// <param name="file1">The file1.</param>
        /// <param name="file2">The file2.</param>
        /// <returns>
        ///   <c>true</c> if [is newest file] [the specified file1]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNewestFile([NotNull] string file1, [NotNull] string file2)
        {
            var f1Date = File.GetLastWriteTime(file1);
            var f2Date = File.GetLastWriteTime(file2);
            return f1Date > f2Date;
        }

        public static bool IsEqualsDateDir([NotNull] string sourceDir, [NotNull] string destDir)
        {
            var dateSrc = Directory.GetLastWriteTime(sourceDir);
            var dateDest = Directory.GetLastWriteTime(destDir);
            return dateSrc.IsEquals(dateDest);
        }

        /// <summary>
        /// Копирование папки. Если файлы уже есть, то заменяются
        /// </summary>
        /// <param name="sourceDir">The source dir.</param>
        /// <param name="destDir">The dest dir.</param>
        [PublicAPI]
        public static void CopyDirectory([NotNull] string sourceDir, string destDir)
        {
            CopyDirectory(sourceDir, destDir, false);
        }

        /// <summary>
        /// Копирование папки.
        /// </summary>
        /// <param name="sourceDir">The source dir.</param>
        /// <param name="destDir">The dest dir.</param>
        /// <param name="onlyNewest">if set to <c>true</c> [only newest].</param>
        [PublicAPI]
        public static void CopyDirectory([NotNull] string sourceDir, string destDir, bool onlyNewest)
        {
            sourceDir = sourceDir.TrimEnd(System.IO.Path.DirectorySeparatorChar);
            destDir = destDir.TrimEnd(System.IO.Path.DirectorySeparatorChar);
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            foreach (var dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, destDir));
            }
            foreach (var sourceFile in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    var destFile = sourceFile.Replace(sourceDir, destDir);
                    // Проверить, что копируемый файл новее
                    if (onlyNewest && File.Exists(destFile) && !IsNewestFile(destFile, sourceFile))
                    {
                        continue;
                    }
                    File.Copy(sourceFile, destFile, true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "CopyDirectory");
                }
            }
        }

        [PublicAPI]
        public static void DeleteDir(string dir)
        {
            if (!Directory.Exists(dir)) return;
            var di = new DirectoryInfo(dir);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (var d in di.GetDirectories())
            {
                d.Delete(true);
            }
        }

        /// <summary>
        /// Возвращает путь к временному файлу
        /// </summary>
        /// <param name="extWithDot">Расширение, если нужно, начиная с точки.</param>
        [NotNull]
        public static string GetTempFile([CanBeNull] string extWithDot = null)
        {
            return System.IO.Path.GetTempPath() + Guid.NewGuid() + (extWithDot ?? ".tmp");
        }

        [NotNull]
        public static string GetTemporaryDirectory()
        {
            var tempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        /// <summary>
        /// Пользовательская папка настроек
        /// </summary>
        /// <returns></returns>
        [NotNull]
        [PublicAPI]
        public static string GetUserPikFolder()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            var companyFolder = General.CompanyNameShortEng;
            var pikAppDataFolder = System.IO.Path.Combine(appData, companyFolder);
            if (!Directory.Exists(pikAppDataFolder))
            {
                Directory.CreateDirectory(pikAppDataFolder);
            }
            return pikAppDataFolder;
        }

        /// <summary>
        /// Путь к пользовательскому файлу настроек плагина
        /// </summary>
        /// <param name="plugin">Имя плагина</param>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Полный путь к файлу</returns>
        [NotNull]
        public static string GetUserPluginFile([NotNull] string plugin, [NotNull] string fileName)
        {
            var pluginFolder = GetUserPluginFolder(plugin);
            return System.IO.Path.Combine(pluginFolder, fileName);
        }

        /// <summary>
        /// Путь к папке плагина
        /// </summary>
        /// <param name="plugin">Имя плагина - имя папки</param>
        /// <returns>Полный путь</returns>
        [NotNull]
        [PublicAPI]
        public static string GetUserPluginFolder([NotNull] string plugin)
        {
            var companyFolder = GetUserPikFolder();
            var pluginFolder = System.IO.Path.Combine(companyFolder, plugin);
            if (!Directory.Exists(pluginFolder))
                Directory.CreateDirectory(pluginFolder);
            return pluginFolder;
        }

        public static void TryDeleteFile(string file)
        {
            try
            {
                File.Delete(file);
            }
            catch
            {
                //
            }
        }
    }
}