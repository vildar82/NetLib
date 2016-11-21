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
         int numberX;
         if (int.TryParse(x, out numberX))
         {
            // x - число numberX
            int numberY;
            if (int.TryParse(y, out numberY))
            {
               // y - число numberY
               return numberX.CompareTo(numberY);
            }
            else
            {
               // y - строка.
               return -1; // число numberX меньше строки y
            }
         }
         else
         {
            // x - строка
            int numberY;
            if (int.TryParse(y, out numberY))
            {
               // y - число numberY
               return 1; // число numberY меньше строки x
            }
            else
            {
               // y - строка.               
               return x.CompareTo(y);
            }
         }
      }
   }
}
