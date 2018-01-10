using System.Collections.Generic;

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
                // ReSharper disable once StringCompareIsCultureSpecific.1
                return int.TryParse(y, out var _) ? 1 : string.Compare(x, y);
                // y - строка.               
            }
        }
    }
}
