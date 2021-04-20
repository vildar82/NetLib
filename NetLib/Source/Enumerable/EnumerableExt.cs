namespace NetLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExt
    {
        /// <summary>
        /// Группировка повторяющихся последовательностей
        /// </summary>
        /// <param name="items">Список</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns>Повторения</returns>
        public static IEnumerable<List<T>> Train<T>(this IEnumerable<T> items, Func<T, string> GetName)
        {
            var train = new HashSet<string>();
            var rail = new List<T>();
            foreach (var item in items)
            {
                var name = GetName(item);
                if (train.Add(name))
                {
                    rail.Add(item);
                }
                else
                {
                    yield return rail;
                    train = new HashSet<string> {name};
                    rail = new List<T> { item };
                }
            }

            yield return rail;
        }

        /// <summary>
        /// Поиск дубликатов имен
        /// </summary>
        /// <param name="items">Саисок элементов</param>
        /// <param name="nameSelector">Выборка имен</param>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <returns>Дублирующиеся имена</returns>
        public static IEnumerable<IGrouping<string, T>> GetDublicateNames<T>(
            this IEnumerable<T> items,
            Func<T, string> nameSelector)
        {
            return items.GroupBy(nameSelector).Where(w => w?.Skip(1).Any() == true);
        }

        /// <summary>
        /// Список совпадающих значений во всех списках
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="lists">Списки значений</param>
        /// <returns>Список совпадаюших значений</returns>
        public static IEnumerable<T> IntersectMany<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            var listNos = lists.Where(w => w != null);
            if (!listNos.Skip(1).Any())
                return listNos.FirstOrDefault();
            return listNos.Skip(1).Aggregate(
                new HashSet<T>(listNos.First()),
                (h, e) =>
                {
                    h.IntersectWith(e);
                    return h;
                }
            );
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
                list.Add(obj);
            return obj;
        }

        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static bool EqualLists<T>(this List<T>? list1, List<T>? list2)
        {
            if (ReferenceEquals(list1, list2))
                return true;

            if (list1 == null || list2 == null)
                return false;

            return list1.All(list2.Contains) && list1.Count == list2.Count;
        }

        public static bool EqualLists<T>(this List<T>? list1, List<T>? list2, IEqualityComparer<T> comparer)
        {
            if (ReferenceEquals(list1, list2)) return true;
            if (list1 == null || list2 == null) return false;
            return list1.All(l1 => list2.Contains(l1, comparer)) && list1.Count == list2.Count;
        }

        public static IEnumerable<TRes> SelectTry<TSource, TRes>(
            this IEnumerable<TSource> list,
            Func<TSource, TRes> selector,
            Action<Exception>? onException = null)
        {
            foreach (var item in list)
            {
                TRes res;
                try
                {
                    res = selector(item);
                    if (res == null) continue;
                }
                catch (Exception ex)
                {
                    onException?.Invoke(ex);
                    continue;
                }

                yield return res;
            }
        }

        public static IEnumerable<TRes> SelectManyNulless<TSource, TRes>(
            this IEnumerable<TSource> list,
            Func<TSource, IEnumerable<TRes>> selector)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            return list.Where(w => w != null).SelectMany(selector).Where(w => w != null);
        }

        /// <summary>
        /// Select без Null
        /// </summary>
        public static IEnumerable<TRes> SelectNulless<TSource, TRes>(this IEnumerable<TSource> list, Func<TSource, TRes> selector)
        {
            return list.Where(w => w != null).Select(selector).Where(w => w != null);
        }

        public static IEnumerable<IEnumerable<T>> SplitParts<T>(this IEnumerable<T> list, int parts)
        {
            var i = 0;
            var splits = list.GroupBy(item => i++ % parts).Select(part => part.AsEnumerable());
            return splits;
        }

        /// <summary>
        /// Преобразует одиночный объект в список из одного объекта (IEnumerable)
        /// </summary>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = General.random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
