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
	    /// Select без Null
	    /// </summary>
	    public static IEnumerable<TRes> SelectNulles<TSource, TRes>(this IEnumerable<TSource> list, Func<TSource, TRes> selector)
	    {
		    return list.Select(selector).Where(w => w != null);
	    }

		/// <summary>
		/// Добавление объекта в коллекцию
		/// </summary>
		/// <typeparam name="T">Тип добавляемого объекта</typeparam>
		/// <param name="obj">Объект добавляемый в коллекцию. Может быть значимым или ссылочным типом.</param>
		/// <param name="list">Коллекция</param>
		/// <returns>Сам объект</returns>
		public static T AddTo<T>(this T obj, ICollection<T> list)
	    {
			// Если это значимый тип, или не дефолтное значение для ссылочных типов (null)
		    if (typeof(T).IsValueType || !EqualityComparer<T>.Default.Equals(obj, default(T)))
		    {
			    list.Add(obj);
		    }
		    return obj;
	    }

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
