using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.IO
{
    public static class Path
    {
        /// <summary>
        /// Возвращает путь к временному файлу
        /// </summary>        
        /// <param name="ext">Расширение, если нужно</param>
        public static string GetTempFile(string extWithDot = null)
        {            
            return System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + (extWithDot ?? ".tmp");
        }
    }
}
