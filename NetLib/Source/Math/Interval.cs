using System;
using JetBrains.Annotations;

namespace NetLib
{
    [PublicAPI]
    public class Interval<T>
        where T : IComparable
    {
        public T Start { get; private set; }

        public T End { get; private set; }

        public object Tag { get; set; }

        public bool HasStart { get; private set; }

        public bool HasEnd { get; private set; }

        private Interval()
        {
        }

        public bool Overlaps(Interval<T> other)
        {
            if (HasStart && other.IsInRange(Start))
                return true;

            return HasEnd && other.IsInRange(End);
        }

        public static Interval<T> Merge([NotNull] Interval<T> int1, [NotNull] Interval<T> int2)
        {
            if (!int1.Overlaps(int2))
                throw new ArgumentException("Interval ranges do not overlap.");

            var hasStart = false;
            var hasEnd = false;
            var start = default(T);
            var end = default(T);

            if (int1.HasStart && int2.HasStart)
            {
                hasStart = true;
                start = (int1.Start.CompareTo(int2.Start) < 0) ? int1.Start : int2.Start;
            }

            if (int1.HasEnd && int2.HasEnd)
            {
                hasEnd = true;
                end = (int1.End.CompareTo(int2.End) > 0) ? int1.Start : int2.Start;
            }

            return CreateInternal(start, hasStart, end, hasEnd);
        }

        private static Interval<T> CreateInternal(T start, bool hasStart, T end, bool hasEnd)
        {
            return new Interval<T>
            {
                Start = start,
                End = end,
                HasEnd = hasEnd,
                HasStart = hasStart,
            };
        }

        public static Interval<T> Create(T start, T end)
        {
            return CreateInternal(start, true, end, true);
        }

        public static Interval<T> CreateLowerBound(T start)
        {
            return CreateInternal(start, true, default, false);
        }

        public static Interval<T> CreateUpperBound(T end)
        {
            return CreateInternal(default, false, end, true);
        }

        /// <summary>
        /// Попадание в диапазон
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsInRange(T item)
        {
            if (HasStart && item.CompareTo(Start) < 0) return false;
            return !HasEnd || item.CompareTo(End) <= 0;
        }
    }
}
