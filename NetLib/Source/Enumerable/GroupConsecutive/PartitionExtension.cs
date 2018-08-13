using System.Collections.Generic;

namespace NetLib
{
    public static class PartitionExtension
    {
        /// <summary>
        /// Группировка последовательно совпадающих значений
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="src">Последовательность значений</param>
        /// <returns>Группировка</returns>
        public static IEnumerable<IEnumerable<T>> GroupConsecutive<T>(this IEnumerable<T> src)
        {
            var grouper = new DuplicateGrouper<T>();
            return grouper.GroupByDuplicate(src);
        }
    }
}