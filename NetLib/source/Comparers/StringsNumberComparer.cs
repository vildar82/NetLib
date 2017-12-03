using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.Comparers
{
   /// <summary>
   /// Сравнение строк как чисел
   /// </summary>
   public class StringsNumberComparer : IComparer<string>
   {
      public int Compare(string x, string y)
      {
            if (int.TryParse(x, out var numberX))
            {
                // x - число numberX
                if (int.TryParse(y, out var numberY))
                {
                    // y - число numberY
                    return numberX.CompareTo(numberY);
                }
                // y - строка.
                return -1; // число numberX меньше строки y
            }
            else
            {
                // x - строка
                return int.TryParse(y, out var numberY) ? 1 : x.CompareTo(y);
                // y - строка.               
            }
        }
   }
}
