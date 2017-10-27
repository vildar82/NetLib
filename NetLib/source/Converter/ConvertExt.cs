using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public static class ConvertExt
    {
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
            if (value is string && typeT == typeof(double))
            {
                value = ((string)value).ToDouble();
                return (T)value;                
            }
            // Округление числа до 4 знаков
            if(value is double &&  typeT == typeof(double))
            {
                value = ((double)value).Round(4);
                return (T)value;
            } 
            if (typeT.IsEnum)
            {
                return (T)value;
            }
            if (typeT == typeof(bool))
            {
                if (value is bool) return (T)value;
                if (value is int valI)
                {
                    value = valI == 1;
                }
                else if (value is string valS)
                {
                    value = valS.EqualsAnyIgnoreCase("Да", "Yes", "1", "+");
                }
                else if (value is double valD)
                {
                    value = Math.Abs(valD - 1) < 0.0001;
                }
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

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

        public static string ColorToString (this System.Drawing.Color color)
        {
            return System.Drawing.ColorTranslator.ToHtml(color); 
        }
        public static System.Drawing.Color StringToColor (this string color)
        {
            if (string.IsNullOrEmpty(color)) return Color.Empty;
            return System.Drawing.ColorTranslator.FromHtml(color);
        }
    }
}
