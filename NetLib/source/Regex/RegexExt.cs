using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetLib
{
    public static class RegexExt
    {
        /// <summary>
        /// Определение числа из строки начинающегося с целого числа
        /// 0 - если число не определено
        /// </summary>        
        public static int StartInt(this string input)
        {
            int value = 0;
            var resRegex = Regex.Match(input, @"^\d+");
            if (resRegex.Success)
            {
                int.TryParse(resRegex.Value, out value);
            }
            return value;
        }
    }
}
