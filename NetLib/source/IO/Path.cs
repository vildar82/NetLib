using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static string GetUserPluginFile(string plugin, string fileName)
        {
            var pluginFolder = GetUserPluginFolder(plugin);
            return System.IO.Path.Combine(pluginFolder, fileName);
        }

        /// <summary>
        /// Путь к папке плагина
        /// </summary>
        /// <param name="plugin">Имя плагина - имя папки</param>
        /// <returns>Полный путь</returns>
        public static string GetUserPluginFolder(string plugin)
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
        /// <param name="ext">Расширение, если нужно</param>
        public static string GetTempFile(string extWithDot = null)
        {            
            return System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + (extWithDot ?? ".tmp");
        }

        public static void CopyDirectory(string sourceDir, string destDir)
        {
            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, destDir));
            }
            foreach (string newPath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(sourceDir, destDir), true);
                }
                catch
                {
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
