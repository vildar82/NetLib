namespace NetLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using JetBrains.Annotations;

    [PublicAPI]
    public static class StringExt
    {
        private static readonly Random random = new Random();

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength) + "...";
        }

        public static bool HasCirilic(this string s)
        {
            return Regex.IsMatch(Regex.Escape(s), "([а-яА-Я])");
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

        public static bool EqualsAny(this string target, IEqualityComparer<string> comparer, [NotNull] IEnumerable<string> values)
        {
            return values.Contains(target, comparer);
        }

        public static bool EqualsAnyIgnoreCase(this string value, [NotNull] params string[] values)
        {
            return EqualsAny(value, StringComparer.OrdinalIgnoreCase, (IEnumerable<string>)values);
        }

        public static bool EqualsAnyIgnoreCase(this string target, [NotNull] IEnumerable<string> values)
        {
            return values.Contains(target, StringComparer.OrdinalIgnoreCase);
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
        /// Сравнение строк с игнорированием спец.символов (неразрывный пробел, перенос строк)
        /// </summary>
        /// <param name="value1">Первая строка</param>
        /// <param name="value2">Вторая строка</param>
        /// <returns>Равны или нет</returns>
        public static bool EqualsIgroreCaseAndSpecChars([NotNull] this string value1, [NotNull] string value2)
        {
            // Удаление спец символов
            var normalS1 = Regex.Replace(value1, @"\s", "");
            var normalS2 = Regex.Replace(value2, @"\s", "");
            return normalS1.Equals(normalS2, StringComparison.OrdinalIgnoreCase);
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
                if (int.TryParse(match.Value, out var v))
                {
                    return Result.Ok(v);
                }
            }
            return Result.Fail<int>($"Не определено целое число из строки - {input}");
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

        public static bool IsBothStringsIsNullOrEmpty([CanBeNull] this string s1, [CanBeNull] string s2)
        {
            return string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2);
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
        /// Соединение списка объектов в одну строку, с разделителем.
        /// Строка из T.ToString()
        /// </summary>
        [NotNull]
        public static string JoinToString<T>([NotNull] this IEnumerable<T> array, string delimeter = ", ")
        {
            return JoinToString(array, t => t.ToString(), delimeter);
        }

        /// <summary>
        /// Объекдинение списка объектов в одну строку, с разделителем и методом получения строки из объекта.
        /// </summary>
        [NotNull]
        public static string JoinToString<T>([NotNull] this IEnumerable<T> array, [NotNull] Func<T, string> getString, string delimeter = ", ")
        {
            return string.Join(delimeter, array.SelectNulless(getString));
        }

        [NotNull]
        public static string RandomString(int length)
        {
            const string chars = " qwerty uiop as df ghj klz xcv bnm AB CD EFG HIJK LMN OP QRS TUVWX YZ0123 456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [NotNull]
        public static string RemoveSpecChars([NotNull] this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }

        [NotNull]
        public static string ToFileName(this DateTime date)
        {
            return date.ToString("MM.dd.yyyy HH.mm.ss");
        }

        public static IEnumerable<string> Split(this string value, int desiredLength)
        {
            var characters = StringInfo.GetTextElementEnumerator(value);
            do
            {
                yield return string.Concat(characters.AsEnumerable<string>().Take(desiredLength));
            }
            while (characters.MoveNext());
        }

        public static IEnumerable<T> AsEnumerable<T>([NotNull] this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
                yield return (T)enumerator.Current;
        }
    }
}
