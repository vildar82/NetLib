using JetBrains.Annotations;
using System;
using System.IO;

namespace NetLib.IO
{
    public static class Path
    {
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
        public static string GetUserPluginFolder([NotNull] string plugin)
        {
            var companyFolder = GetUserPikFolder();
            var pluginFolder = System.IO.Path.Combine(companyFolder, plugin);
            if (!Directory.Exists(pluginFolder))
                Directory.CreateDirectory(pluginFolder);
            return pluginFolder;
        }

        /// <summary>
        /// Пользовательская папка настроек
        /// </summary>
        /// <returns></returns>
        [NotNull]
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
            return System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + (extWithDot ?? ".tmp");
        }

        public static void CopyDirectory([NotNull] string sourceDir, string destDir)
        {
            foreach (var dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, destDir));
            }
            foreach (var newPath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(sourceDir, destDir), true);
                }
                catch
                {
                    //
                }
            }
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
