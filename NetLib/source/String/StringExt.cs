using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetLib
{
    public static class StringExt
    {
        /// <summary>
        /// IndexOf(toCheck, comp) >= 0
        /// </summary>        
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// Удаление разделителей строк и др. \r\n?|\n
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ClearString(this string input)
        {
            //return Regex.Replace(input, @"\r\n?|\n", "");
            return input.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
        }        

        /// <summary>
        /// Формат строки с аргументами string.Format();
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string f(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}