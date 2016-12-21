using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public static class EnumerableExt
    {
        /// <summary>
        /// Преобразует одиночный объект в список из одного объекта (IEnumerable)
        /// </summary>                        
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
