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

		public static bool EqualLists<T>(this List<T> list1, List<T> list2)
		{
			if (ReferenceEquals(list1, list2)) return true;
			if (list1 == null || list2 == null) return false;
			return list1.All(list2.Contains) && list1.Count == list2.Count;
		}
	}
}
