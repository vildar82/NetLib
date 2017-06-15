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
        /// Есть ли в строке кирилические символы (русские буквы)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasCyrilic(this string input)
        {
            return Regex.IsMatch(input, @"\p{IsCyrillic}");
        }

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
            return input.Trim().Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace(Convert.ToChar(160),' ');
        }                

        /// <summary>
        /// Определение числа из строки начинающейся числом.
        /// Например: "100 шт." = 100
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Result<int> GetStartInteger(this string input)
        {
            int value = 0;
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

        /// <summary>
        /// Сравнение строк с игнорированием спец.символов (неразрывный пробел, перенос строк)
        /// </summary>
        /// <param name="value1">Первая строка</param>
        /// <param name="value2">Вторая строка</param>
        /// <returns>Равны или нет</returns>
        public static bool EqualsIgroreCaseAndSpecChars (this string value1, string value2)
        {
            // Удаление спец символов
            string normalS1 = Regex.Replace(value1, @"\s", "");
            string normalS2 = Regex.Replace(value2, @"\s", "");
            return normalS1.Equals(normalS2, StringComparison.OrdinalIgnoreCase);
        }

	    private static readonly Random random = new Random();
	    public static string RandomString(int length)
	    {
		    const string chars = " qwerty uiop as df ghj klz xcv bnm AB CD EFG HIJK LMN OP QRS TUVWX YZ0123 456789";
		    return new string(Enumerable.Repeat(chars, length)
			    .Select(s => s[random.Next(s.Length)]).ToArray());
	    }
	}
}