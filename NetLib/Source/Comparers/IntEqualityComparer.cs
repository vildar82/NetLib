namespace NetLib.Comparers
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Сравнение чисел
    /// </summary>
    [PublicAPI]
    public class IntEqualityComparer : IEqualityComparer<int>
    {
        private readonly int threshold;

        public IntEqualityComparer(int threshold = 1)
        {
            this.threshold = threshold;
        }

        public bool Equals(int x, int y)
        {
            return Math.Abs(x - y) <= threshold;
        }

        public int GetHashCode(int obj)
        {
            return 0;
        }
    }
}