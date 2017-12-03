using System;
using System.Collections.Generic;
using System.Linq;

namespace NetLib
{
    public static class EnumerableExt
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static IEnumerable<IEnumerable<T>> SplitParts<T>(this IEnumerable<T> list, int parts)
        {
            var i = 0;
            var splits = list.GroupBy(item => i++ % parts).Select(part => part.AsEnumerable());
            return splits;
        }

        public static IEnumerable<TRes> SelectManyNulless<TSource, TRes>(this IEnumerable<TSource> list,
            Func<TSource, IEnumerable<TRes>> selector)
        {
            return list.Where(w => w != null && selector(w) != null).SelectMany(selector).Where(w => w != null);
        }

        /// <summary>
        /// Select без Null
        /// </summary>
        public static IEnumerable<TRes> SelectNulless<TSource, TRes>(this IEnumerable<TSource> list, Func<TSource, TRes> selector)
        {
            return list.Where(w => w != null).Select(selector).Where(w => w != null);
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
            if (typeof(T).IsValueType || !EqualityComparer<T>.Default.Equals(obj, default))
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
