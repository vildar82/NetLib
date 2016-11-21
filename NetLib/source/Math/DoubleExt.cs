using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public static class DoubleExt
    {
        public static double Round (this double value, int digits=4)
        {
            return Math.Round(value, digits);
        }
    }
}
