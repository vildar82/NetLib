using JetBrains.Annotations;
using NetLib.WPF.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetLib
{
    public static class EnumExt
    {
        /// <summary>
        /// Атрибут Description из значения enum
        /// </summary>
        /// <param name="enumValue">Enum значение</param>
        /// <returns>Подаись в атрибуте Descrip</returns>
        [CanBeNull]
        public static string Description([CanBeNull] this object enumValue)
        {
            return enumValue == null ? null : EnumDescriptionTypeConverter.GetEnumDescription(enumValue);
        }

        [NotNull]
        [PublicAPI]
        public static List<string> GetEnumDesciptionValues([NotNull] this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().Select(s => s.Description()).ToList();
        }

        /// <summary>
        /// Есть ли хоть один совпадающий флаг в enum1 и enum2
        /// </summary>
        [PublicAPI]
        public static bool HasAny<T>(this T enum1, T enum2) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("Это не enum");
            var v1 = Convert.ToInt32(enum1);
            var v2 = Convert.ToInt32(enum2);
            return v1 == 0 || v2 == 0 || (v1 & v2) > 0;
        }

        /// <summary>
        /// Конвертация строки в соответствующее значение перечисления enum
        /// Выбрасывает исключение при несоответствии
        /// </summary>
        [NotNull]
        [PublicAPI]
        public static T ToEnum<T>([NotNull] this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Конвертация строки в соответствующее значение перечисления enum.
        /// Ичключение не выбрасывапется. (если, только, T не структура)
        /// </summary>
        [PublicAPI]
        public static T ToEnum<T>([CanBeNull] this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }
    }
}