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
