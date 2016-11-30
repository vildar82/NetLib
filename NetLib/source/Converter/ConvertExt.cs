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
            if (value is string && typeof(T) == typeof(double))
            {
                value = ((string)value).ToDouble().ToString();
                //value = ((string)value).Replace(",", ".");
                //culture = new CultureInfo("en-US");
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
