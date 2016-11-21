using System;
using System.Collections.Generic;

namespace NetLib.Comparers
{
   // Сравнение чисел
   public class IntEqualityComparer : IEqualityComparer<int>
   {
      private readonly int threshold;

      public IntEqualityComparer (int threshold = 1)
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