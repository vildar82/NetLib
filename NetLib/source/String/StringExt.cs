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
        /// Соединение списка объектов в одну строку, с разделителем. 
        /// Строка из T.ToString()
        /// </summary>
        public static string JoinToString<T>(this IEnumerable<T> array, string delimeter = ",")
        {
            return JoinToString(array, t=> t.ToString(), delimeter);
        }

        /// <summary>
        /// Объекдинение списка объектов в одну строку, с разделителем и методом получения строки из объекта.
        /// </summary>
        public static string JoinToString<T>(this IEnumerable<T> array, Func<T, string> getString, string delimeter =",")
        {
            return string.Join(delimeter, array.SelectNulless(getString));
        }

	    public static bool IsNullOrEmpty(this string str)
	    {
		    return string.IsNullOrEmpty(str);
	    }

	    public static bool IsNullOrWhiteSpace(this string str)
	    {
		    return string.IsNullOrWhiteSpace(str);
	    }

		/// <summary>
		/// Сравнение строк без учета регистра. 
		/// Внимание! null, "", и строка только с пробелами считаются равными!
		/// </summary>
		public static bool EqualsIgnoreCase(this string string1, string string2)
		{
			return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase) ||
			       IsBothStringsIsNullOrEmpty(string1, string2);
		}

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

        public static bool EqualsAny(this string value, IEqualityComparer<string> comparer, params string[] values)
        {
            return EqualsAny(value, comparer, (IEnumerable<string>)values);
        }
        public static bool EqualsAnyIgnoreCase(this string value, params string[] values)
        {
            return EqualsAny(value, StringComparer.OrdinalIgnoreCase, (IEnumerable<string>)values);
        }
        public static bool EqualsAny(this string target, IEqualityComparer<string> comparer, IEnumerable<string> values)
        {
            return values.Contains(target, comparer);
        }
        public static bool EqualsAnyIgnoreCase(this string target, IEnumerable<string> values)
        {
            return values.Contains(target, StringComparer.OrdinalIgnoreCase);
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
            var match = Regex.Match(input, @"^\d*");
            if (match.Success)
            {
                if (int.TryParse(match.Value, out int value))
                {
                    return Result.Ok(0);
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

        public static string RemoveSpecChars(this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }

	    public static bool IsBothStringsIsNullOrEmpty(this string s1, string s2)
	    {
		    return string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2);
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