using System;
using System.Collections.Generic;
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
            if (value is string && typeT == typeof(double))
            {
                value = ((string)value).ToDouble();
                return (T)value;                
            }
            else  if(value is double &&  typeT == typeof(double))
            {
                value = ((double)value).Round(4);
                return (T)value;
            } 
            else if (typeT.IsEnum)
            {
                return (T)value;
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
    }
}
