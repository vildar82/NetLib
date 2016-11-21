using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetLib
{
    public static class StringHelper
    {
        /// <summary>
        /// Определение числа из строки начинающейся числом.
        /// Например: "100 шт." = 100
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Result<int> GetStartInteger (string input)
        {
            int value =0;
            var match = Regex.Match(input, @"^\d*");
            if (match.Success)
            {
                if (int.TryParse(match.Value, out value))
                {
                    return Result.Ok(value);
                }
            }
            return Result.Fail<int>($"Не определено целое число из строки - {input}");
        }
    }
}
