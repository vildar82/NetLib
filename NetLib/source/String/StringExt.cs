using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetLib
{
    public static class StringExt
    {
        [NotNull]
        public static string ToFileName(this DateTime date)
        {
            return date.ToString("MM.dd.yyyy HH.mm.ss");
        }

        /// <summary>
        /// Соединение списка объектов в одну строку, с разделителем. 
        /// Строка из T.ToString()
        /// </summary>
        [NotNull]
        public static string JoinToString<T>([NotNull] this IEnumerable<T> array, string delimeter = ",")
        {
            return JoinToString(array, t => t.ToString(), delimeter);
        }

        /// <summary>
        /// Объекдинение списка объектов в одну строку, с разделителем и методом получения строки из объекта.
        /// </summary>
        [NotNull]
        public static string JoinToString<T>([NotNull] this IEnumerable<T> array, [NotNull] Func<T, string> getString, string delimeter = ",")
        {
            return string.Join(delimeter, array.SelectNulless(getString));
        }
        
        public static bool IsNullOrEmpty([CanBeNull] this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace([CanBeNull] this string str)
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
        public static bool HasCyrilic([NotNull] this string input)
        {
            return Regex.IsMatch(input, @"\p{IsCyrillic}");
        }

        /// <summary>
        /// IndexOf(toCheck, comp) >= 0
        /// </summary>        
        public static bool Contains([CanBeNull] this string source, [NotNull] string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static bool EqualsAny(this string value, [NotNull] params string[] values)
        {
            return values.Contains(value);
        }
        public static bool EqualsAny(this string value, IEqualityComparer<string> comparer, [NotNull] params string[] values)
        {
            return EqualsAny(value, comparer, (IEnumerable<string>)values);
        }
        public static bool EqualsAnyIgnoreCase(this string value, [NotNull] params string[] values)
        {
            return EqualsAny(value, StringComparer.OrdinalIgnoreCase, (IEnumerable<string>)values);
        }
        public static bool EqualsAny(this string target, IEqualityComparer<string> comparer, [NotNull] IEnumerable<string> values)
        {
            return values.Contains(target, comparer);
        }
        public static bool EqualsAnyIgnoreCase(this string target, [NotNull] IEnumerable<string> values)
        {
            return values.Contains(target, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Удаление разделителей строк и др. \r\n?|\n
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NotNull]
        public static string ClearString([NotNull] this string input)
        {
            //return Regex.Replace(input, @"\r\n?|\n", "");
            return input.Trim().Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace(Convert.ToChar(160), ' ');
        }

        /// <summary>
        /// Определение числа из строки начинающейся числом.
        /// Например: "100 шт." = 100
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NotNull]
        public static Result<int> GetStartInteger([NotNull] this string input)
        {
            var match = Regex.Match(input, @"^\d*");
            if (match.Success)
            {
                if (int.TryParse(match.Value, out var _))
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
        public static bool EqualsIgroreCaseAndSpecChars([NotNull] this string value1, [NotNull] string value2)
        {
            // Удаление спец символов
            string normalS1 = Regex.Replace(value1, @"\s", "");
            string normalS2 = Regex.Replace(value2, @"\s", "");
            return normalS1.Equals(normalS2, StringComparison.OrdinalIgnoreCase);
        }

        [NotNull]
        public static string RemoveSpecChars([NotNull] this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }

        public static bool IsBothStringsIsNullOrEmpty([CanBeNull] this string s1, [CanBeNull] string s2)
        {
            return string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2);
        }

        private static readonly Random random = new Random();
        [NotNull]
        public static string RandomString(int length)
        {
            const string chars = " qwerty uiop as df ghj klz xcv bnm AB CD EFG HIJK LMN OP QRS TUVWX YZ0123 456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}