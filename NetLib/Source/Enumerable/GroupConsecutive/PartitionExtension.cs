namespace NetLib
{
    using System;
    using System.Collections.Generic;

    public static class PartitionExtension
    {
        /// <summary>
        /// Группировка последовательно совпадающих значений
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="src">Последовательность значений</param>
        /// <returns>Группировка</returns>
        public static IEnumerable<IEnumerable<T>> GroupConsecutive<T, Tkey>(this IEnumerable<T> src, Func<T, Tkey> keySelector)
        {
            var grouper = new DuplicateGrouper<T, Tkey>();
            return grouper.GroupByDuplicate(src, keySelector);
        }
    }
}