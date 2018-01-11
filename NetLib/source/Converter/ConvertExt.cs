using JetBrains.Annotations;
using System;
using System.Drawing;

namespace NetLib
{
    public static class ConvertExt
    {
        [PublicAPI]
        public static string ColorToString(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }

        /// <summary>
        /// Приведение типа объекта, к заданному типу, через Convert.ChangeType.
        /// Если value строка, а T double, то учитывается разделитель ,
        /// </summary>
        /// <typeparam name="T">Требуемый тип</typeparam>
        /// <param name="value">Значение</param>
        /// <returns>Значение приведенное к заданному типу.</returns>
        /// <exception cref="InvalidCastException">Это и другие исключения от Convert.ChangeType</exception>
        public static T GetValue<T>(this object value)
        {
            var typeT = typeof(T);

            // Из строки в число - разделитель точка или запятая
            if (value is string s)
            {
                if (s.IsNullOrEmpty()) return default;
                if (typeT == typeof(double))
                {
                    value = ((string)value).ToDouble();
                    return (T)value;
                }
            }
            // Округление числа до 4 знаков
            if (value is double && typeT == typeof(double))
            {
                value = ((double)value).Round();
                return (T)value;
            }
            if (typeT.IsEnum)
            {
                return (T)value;
            }
            if (typeT == typeof(bool))
            {
                switch (value)
                {
                    case bool _:
                        return (T)value;

                    case int valI:
                        value = valI == 1;
                        break;

                    case string valS:
                        value = valS.EqualsAnyIgnoreCase("Да", "Yes", "1", "+");
                        break;

                    case double valD:
                        value = Math.Abs(valD - 1) < 0.0001;
                        break;
                }
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

        [PublicAPI]
        public static Color StringToColor([CanBeNull] this string color)
        {
            return string.IsNullOrEmpty(color) ? Color.Empty : ColorTranslator.FromHtml(color);
        }

        [PublicAPI]
        public static T TryGetValue<T>(this object value, T defaultValue)
        {
            try
            {
                return value.GetValue<T>();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}